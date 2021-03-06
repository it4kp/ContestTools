using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeParsing;
using ContestToolsAddIn.Forms;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.CommandBars;
using Constants = EnvDTE.Constants;
using Document = EnvDTE.Document;

namespace ContestToolsAddIn
{
	/// <summary>
	/// Main plugin class
	/// </summary>
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		#region Fields

		private DTE2 _applicationObject;
		private AddIn _addInInstance;
		private SettingsXml _settings;
		private DocumentEvents _documentEvents;
		private CSharpCompilation _compilation;
		private CodeImporter _codeImporter;
		private FileSystemWatcher _watcher;

		#endregion

		#region Methods

		#region IDTExtensibility2 implementation

		public void OnConnection( object application, ext_ConnectMode connectMode, object addInInst, ref Array custom )
		{
			try
			{
				_applicationObject = (DTE2)application;
				_addInInstance = (AddIn)addInInst;

				LoadSettings();

				CreateLibrary();

				AddCustomUI();

				_documentEvents = _applicationObject.Events.DocumentEvents;
				_documentEvents.DocumentSaved += DocumentEventsOnDocumentSaved;

				// Watch library directory for changes
				// and when they come, update im-memory compilation
				if ( !string.IsNullOrEmpty( _settings.LibraryRootDirectory )
						&& Directory.Exists( _settings.LibraryRootDirectory ) )
				{
					_watcher = new FileSystemWatcher( _settings.LibraryRootDirectory, "*.cs" ) { IncludeSubdirectories = true };
					_watcher.Changed += ( sender, args ) => CreateLibrary();
					_watcher.Created += ( sender, args ) => CreateLibrary();
					_watcher.Deleted += ( sender, args ) => CreateLibrary();
					_watcher.Renamed += ( sender, args ) => CreateLibrary();

					_watcher.EnableRaisingEvents = true;
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Error during initializing ContestTools add-in. " + ex );
			}
		}

		private static readonly object Sync = new object();

		/// <summary>
		/// Create in-memory compilation of library
		/// </summary>
		private void CreateLibrary()
		{
			if ( string.IsNullOrEmpty( _settings.LibraryRootDirectory )
					|| !Directory.Exists( _settings.LibraryRootDirectory ) )
				return;
			try
			{
				lock ( Sync )
				{
					var builder = new CompilationBuilder();

					// adding all *.cs files
					foreach ( var path in Directory.GetFiles( _settings.LibraryRootDirectory, "*.cs", SearchOption.AllDirectories ) )
					{
						builder.AddSourceFile( path );
					}
					_compilation = builder.Build();

					var errors = builder.ValidateCompilation( _compilation );
					if ( errors != null && errors.Any() )
					{
						MessageBox.Show( "There were compilation errors of library: " + Environment.NewLine
														+ string.Join( Environment.NewLine, errors ) );
					}

					_codeImporter = new CodeImporter( _compilation,
						location => File.ReadAllText( location.FileName ).Substring( location.Span.Start, location.Span.Length ) );
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Error during library compilation: " + ex.Message );
			}
		}

		private void DocumentEventsOnDocumentSaved( Document document )
		{
			if ( !document.Name.EndsWith( ".cs", StringComparison.InvariantCultureIgnoreCase ) )
				return;
			var source = File.ReadAllText( document.FullName );
			if ( !source.StartsWith( "//CONTEST_TOOL_SUBMIT:" ) )
				return;
			document.DTE.StatusBar.Text = "Generating file to submit...";
			int p = source.IndexOf( Environment.NewLine );
			if ( p == -1 ) return;
			var submitFileName = source.Substring( 22, p - 22 );
			source = source.Substring( p + 1 ); // stripping hint file name

			source = MakeAllReplacements( source, submitFileName );

			lock ( Sync )
			{
				if ( _compilation != null )
				{
					source = _codeImporter.ImportCode( source );
				}

				var errorFile = Path.Combine( Path.GetDirectoryName( submitFileName ), "compilationLog.txt" );
				var errors = _codeImporter.Validate( source );
				if ( errors.Any() )
				{
					File.WriteAllText( errorFile, string.Join( Environment.NewLine, errors.ToArray() ) );

					document.DTE.StatusBar.Text = "COMPILATION ERROR !!!";
				}
				else
				{
					File.Delete( errorFile );
				}
			}
			File.WriteAllText( submitFileName, source );
		}

		private string MakeAllReplacements( string source, string submitFileName )
		{
			const string OUT_PATH_MARKER = "%CONTEST_TOOL_SUBMIT_PATH%";
			const string OUT_FILE_MARKER = "%CONTEST_TOOL_SUBMIT_FILE%";
			var path = Path.GetDirectoryName( submitFileName );

			source = source.Replace( OUT_PATH_MARKER, path );
			source = source.Replace( OUT_FILE_MARKER, submitFileName );

			return source;
		}

		public void OnDisconnection( ext_DisconnectMode disconnectMode, ref Array custom )
		{

		}

		public void OnAddInsUpdate( ref Array custom )
		{
		}

		public void OnStartupComplete( ref Array custom )
		{
		}

		public void OnBeginShutdown( ref Array custom )
		{
		}

		#endregion

		#region IDTCommandTarget implementation

		/// <summary>
		/// Handle state of commands
		/// </summary>
		public void QueryStatus( string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText )
		{
			if ( neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone )
			{
				if ( commandName == "ContestToolsAddIn.Connect.ContestToolsSettings" )
				{
					status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
					return;
				}
				if ( commandName == "ContestToolsAddIn.Connect.ContestToolsNewProblem"
					|| commandName == "ContestToolsAddIn.Connect.ContestToolsRecentProblems" )
				{
					if ( !_applicationObject.Solution.IsOpen
						|| _applicationObject.Solution.Projects.Count == 0 )
					{
						status = vsCommandStatus.vsCommandStatusUnsupported;
					}
					else
					{
						status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
					}
					return;
				}
			}
		}

		/// <summary>
		/// Exceution of commands
		/// </summary>
		public void Exec( string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled )
		{
			handled = false;
			if ( executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault )
			{
				if ( commandName == "ContestToolsAddIn.Connect.ContestToolsSettings" )
				{
					handled = true;
					new SettingsForm( _settings ).ShowDialog();
					return;
				}
				if ( commandName == "ContestToolsAddIn.Connect.ContestToolsNewProblem" )
				{
					handled = true;
					AddNewProblem();
					return;
				}
				if ( commandName == "ContestToolsAddIn.Connect.ContestToolsRecentProblems" )
				{
					handled = true;
					RecentFilesDialog();
					return;
				}
			}
		}

		#endregion

		#region Helper methods

		/// <summary>
		/// Loading settings from file
		/// </summary>
		private void LoadSettings()
		{
			var settingsFileName = GetSettingsFileName();
			if ( File.Exists( settingsFileName ) )
			{
				try
				{
					_settings = SettingsXml.Load( settingsFileName );
				}
				catch ( Exception ex )
				{
					MessageBox.Show( "Error while loading settings from " + settingsFileName + ". " + ex );
				}
			}
			else
			{
				_settings = new SettingsXml();
			}
		}

		private static string GetSettingsFileName()
		{
			if ( !string.IsNullOrEmpty( Properties.Settings.Default.SettingPath ) &&
					File.Exists( Properties.Settings.Default.SettingPath ) )
				return Properties.Settings.Default.SettingPath;
			return Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
				"ContestToolsSettings.xml" );
		}

		/// <summary>
		/// Saving settings to file
		/// </summary>
		private void SaveSettings()
		{
			var settingsFileName = GetSettingsFileName();
			try
			{
				_settings.Save( settingsFileName );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Error while saving settings to " + settingsFileName + ". " + ex );
			}
		}

		private void RecentFilesDialog()
		{
			var dialog = new RecentFilesForm( _settings );
			if ( dialog.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty( dialog.SelectedFile ) ) return;
			if ( !File.Exists( dialog.SelectedFile ) ) return;

			var sb = (SolutionBuild2)_applicationObject.Solution.SolutionBuild;
			var startupProjects = (Array)sb.StartupProjects;
			if ( startupProjects.Length != 1 )
			{
				MessageBox.Show( "There must be exactly one startup project in the solution" );
				return;
			}

			// remove all *.cs files from project
			bool fileExists = false;
			var startupProject = _applicationObject.Solution.Item( startupProjects.GetValue( 0 ) );
			foreach ( ProjectItem item in startupProject.ProjectItems )
			{
				if ( item.Name.EndsWith( ".cs", StringComparison.InvariantCultureIgnoreCase ) )
				{
					if ( item.FileCount == 1 && item.FileNames[0] == dialog.SelectedFile )
					{
						// the file we selected is already added
						fileExists = true;
					}
					else
					{
						item.Remove();
					}
				}
			}

			if ( fileExists )
				return;

			// add selected file

			try
			{
				var projectItem = startupProject.ProjectItems.AddFromFile( dialog.SelectedFile );
				startupProject.Save();
				var window = projectItem.Open( Constants.vsViewKindCode );
				window.Visible = true;
				window.Activate();

				UpdateSettings( dialog.SelectedFile );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

		private void AddNewProblem()
		{
			var settingsErrors = _settings.Validate();
			if ( settingsErrors.Count > 0 )
			{
				MessageBox.Show( string.Format( "Incorrect settings:\r\n{0}", string.Join( Environment.NewLine, settingsErrors.ToArray() ) ) );
				return;
			}
			var dialog = new NewProblemForm( _settings );
			if ( dialog.ShowDialog() != DialogResult.OK ) return;

			var sb = (SolutionBuild2)_applicationObject.Solution.SolutionBuild;
			var startupProjects = (Array)sb.StartupProjects;
			if ( startupProjects.Length != 1 )
			{
				MessageBox.Show( "There must be exactly one startup project in the solution" );
				return;
			}
			var startupProject = _applicationObject.Solution.Item( startupProjects.GetValue( 0 ) );

			// remove all *.cs files
			foreach ( ProjectItem item in startupProject.ProjectItems )
			{
				if ( item.Name.EndsWith( ".cs", StringComparison.InvariantCultureIgnoreCase ) )
				{
					item.Remove();
				}
			}

			try
			{
				// add new file
				var projectItem = startupProject.ProjectItems.AddFromFile( dialog.SourceFileName );
				startupProject.Save();
				var window = projectItem.Open( Constants.vsViewKindCode );
				window.Visible = true;
				window.Activate();

				UpdateSettings( dialog.SourceFileName );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

		/// <summary>
		/// Update settings
		/// </summary>
		private void UpdateSettings( string mostRecentFileName )
		{
			var settingsChanged = false;
			if ( _settings.RecentFiles == null )
			{
				_settings.RecentFiles = new List<string>();
				settingsChanged = true;
			}
			if ( !_settings.RecentFiles.Contains( mostRecentFileName ) )
			{
				_settings.RecentFiles.Insert( 0, mostRecentFileName );
				settingsChanged = true;
			}
			else if ( _settings.RecentFiles[0] != mostRecentFileName )
			{
				settingsChanged = true;
				_settings.RecentFiles.Remove( mostRecentFileName );
				_settings.RecentFiles.Insert( 0, mostRecentFileName );
			}

			if ( _settings.RecentFiles.Count > 15 )
			{
				_settings.RecentFiles = _settings.RecentFiles.Take( 15 ).ToList();
				settingsChanged = true;
			}

			if ( settingsChanged )
				_settings.Save();
		}

		private void AddCustomUI()
		{
			var contextGUIDS = new object[] { };
			var commands = (Commands2)_applicationObject.Commands;
			var commandBars = (CommandBars)_applicationObject.CommandBars;

			Command settingsCommand = null;
			Command newProblemCommand = null;
			Command recentCommand = null;
			CommandBar myToolBar = null;

			try
			{
				myToolBar = commandBars["ContestTools"];
			}
			catch { }
			if ( myToolBar == null )
				myToolBar = commandBars.Add( "ContestTools" );

			try
			{
				recentCommand = _applicationObject.Commands.Item( _addInInstance.ProgID + "." + "ContestToolsRecentProblems" );
			}
			catch
			{
			}
			if ( recentCommand == null )
			{
				recentCommand = commands.AddNamedCommand2( _addInInstance, "ContestToolsRecentProblems", "Recent problems",
																										"Recent problems", true, 540,
																										ref contextGUIDS );
				var button = (CommandBarButton)recentCommand.AddControl( myToolBar );
				button.Style = MsoButtonStyle.msoButtonIcon;
			}

			try
			{
				settingsCommand = _applicationObject.Commands.Item( _addInInstance.ProgID + "." + "ContestToolsSettings" );
			}
			catch
			{
			}
			if ( settingsCommand == null )
			{
				settingsCommand = commands.AddNamedCommand2( _addInInstance, "ContestToolsSettings", "Contest Tools Settings",
																										"Opens Contest Tools settings dialog", true, 548,
																										ref contextGUIDS );
				var button = (CommandBarButton)settingsCommand.AddControl( myToolBar );
				button.Style = MsoButtonStyle.msoButtonIcon;
			}

			try
			{
				newProblemCommand = _applicationObject.Commands.Item( _addInInstance.ProgID + "." + "ContestToolsNewProblem" );
			}
			catch
			{
			}
			if ( newProblemCommand == null )
			{
				newProblemCommand = commands.AddNamedCommand2( _addInInstance, "ContestToolsNewProblem", "Add new problem",
																										"Add new problem", true, 240,
																										ref contextGUIDS );
				var button = (CommandBarButton)newProblemCommand.AddControl( myToolBar );
				button.Style = MsoButtonStyle.msoButtonIcon;
			}
		}

		#endregion

		#endregion
	}
}