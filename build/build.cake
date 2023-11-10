#addin nuget:https://api.nuget.org/v3/index.json?package=Cake.Npm&version=2.0.0
#addin nuget:https://api.nuget.org/v3/index.json?package=Cake.7zip&version=2.0.0
#addin nuget:?package=System.Text.Json&version=4.6.0&loaddependencies=true
#tool nuget:https://api.nuget.org/v3/index.json?package=NuGet.CommandLine&version=5.9.1

// Cake script
// Relies on environment variables
// Recommend to use with an orchestrator, e.g. AppVeyor.
var solutions = GetFiles("../**/*.sln");

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument( "target", "Build" );

// Specifies package version directly, setting
// major, minor and patch versions plus suffix
var tag = GitHubActions.Environment.Workflow.Ref;
var buildId = Argument( "build", "0" );
var commit = Argument( "commit", "" );
var configuration = Argument( "configuration", "Release" );
var majorVersion = "0";
var minorVersion = "0";
var patchVersion = "0";
var version = "1.0.0";
var externalPublish = false; // If true and build type "release" then publish packages to nuget feed
var suffix = Argument( "suffix", "" );

//Ensures environment is clean of any previous build artifacts
CleanEnvironment();

// Initialize variables according to the Git tag
SetupFromTag( tag );
GenerateAssemblyInfo();

//////////////////////////////////////////////////////////////////////
// PROCEDURES AND FUNCTIONS
//////////////////////////////////////////////////////////////////////

void CleanEnvironment() {
  var dir = "./artifacts";

  if ( !DirectoryExists( dir ) )
  {
    CreateDirectory( dir );
  }
  else {
    DeleteDirectory( dir, new DeleteDirectorySettings {
    Recursive = true
    } );
  }
}

// Set the environment and the variables according to the tag it is running on
void SetupFromTag( string tag ) {
  var releasePattern = new System.Text.RegularExpressions.Regex( @"((?<type>.+)/)?(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)(?<suffix>-.+)?" );

  if ( releasePattern.IsMatch( tag ?? string.Empty ) ) {
    Information( "Release detected, setting package version..." );
    var matches = releasePattern.Match( tag ?? string.Empty );
    majorVersion = matches.Groups["major"].Value;
    minorVersion = matches.Groups["minor"].Value;
    patchVersion = matches.Groups["patch"].Value;
    suffix = matches.Groups["suffix"].Value;
    if ( !string.IsNullOrEmpty( suffix ) && suffix.StartsWith( "-" ) ) {
      suffix = suffix.Substring( 1 );
    }
    if( string.IsNullOrEmpty( suffix ) || suffix.StartsWith( "preview" ) ) {
      externalPublish = true;
    }
    version = $"{majorVersion}.{minorVersion}.{patchVersion}";
  }

  Information( "Package version=" + majorVersion + "." + minorVersion + "." + patchVersion);
  Information( "Version suffix=" + suffix );
}

void GenerateAssemblyInfo() {

  CreateAssemblyInfo( "./GlobalAssemblyInfo.cs", new AssemblyInfoSettings {
    Version = version,
    FileVersion = version,
    InformationalVersion = version,
    Company = "Configit",
    Copyright = $"Copyright Configit A/S {DateTime.Now.ToString( "yyyy" )}",
    Configuration = configuration,
    ComVisible = false
  } );
}


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task( "EnsureFeeds" )
  .Does( () =>  {
        
    if ( GitHubActions.IsRunningOnGitHubActions ) {
      Dictionary<string, NuGetSourcesSettings> settings = new Dictionary<string, NuGetSourcesSettings> {
        { 
          "Configit-Sdk", new NuGetSourcesSettings() {
            UserName  =  EnvironmentVariable("CONFIGIT_SDK_PACKAGES_READ_USERNAME"),
            Password = EnvironmentVariable("CONFIGIT_SDK_PACKAGES_READ_TOKEN"),
            IsSensitiveSource = true,
            Verbosity = NuGetVerbosity.Detailed
          }
        },
        { 
          "Configit", new NuGetSourcesSettings() {
            UserName  =  "bob",
            Password = EnvironmentVariable("BOB_PASSWORD"),
            IsSensitiveSource = true,
            Verbosity = NuGetVerbosity.Detailed
          }
        },
        {   
          "Github", new NuGetSourcesSettings() {
            UserName  =  EnvironmentVariable("GITHUB_ACTOR"),
            Password = EnvironmentVariable("GITHUB_TOKEN"),
            IsSensitiveSource = true,
            Verbosity = NuGetVerbosity.Detailed
          } 
        }
      };

      Dictionary<string, string> feeds = new Dictionary<string, string>() {
        { "Configit-Sdk","https://nuget.pkg.github.com/configit-sdk/index.json" },
        { "Configit","https://nuget.configit.com/nuget/Configit/" },
        { "Github","https://nuget.pkg.github.com/configit-services/index.json" },
      };

      foreach ( var feed in feeds ) {
        if ( !NuGetHasSource( feed.Value ) ) {
          Information( "Adding "+feed.Key );
          if ( settings.ContainsKey( feed.Key ) ) {
            NuGetAddSource( feed.Key, feed.Value, settings[feed.Key] );
          } else {
            NuGetAddSource( feed.Key, feed.Value );
          }
        } 
      }
    }
  } );

// Clean
Task( "Clean" )
  .Does( () => {
    foreach ( var dir in GetDirectories( "**/obj/*" ) ) {
      CleanDirectory( dir );
    }
    foreach ( var dir in GetDirectories( "**/bin/*" ) ) {
      CleanDirectory( dir );
    }
  } );

// RestoreOnly
Task( "RestoreOnly" )
  .Does( () => {
	foreach(var solution in solutions) {
      NuGetRestore( solution );
	}
  } );

// NpmRestoreOnly
Task( "NpmRestoreOnly" )
  .Does( () => {
    NpmInstall( s => s.FromPath( "../car-reg-ui" ) );
  } );

// BuildOnly
Task( "BuildOnly" )
  .Does( () => {	
    foreach(var solution in solutions)	{
    Information("Build solution");
    MSBuild( solution, settings =>
      settings
        .SetVerbosity( Verbosity.Quiet )
        .SetConfiguration( configuration )
        .WithProperty( "MajorVersion", majorVersion )
        .WithProperty( "MinorVersion", minorVersion )
        .WithProperty( "PatchVersion", patchVersion )
        .WithProperty( "BuildVersion", buildId )
        .WithProperty( "VersionSuffix", suffix )
        .WithProperty( "Commit", commit ) );	
    }
  } );

// UiBuildOnly
Task( "UiBuildOnly" )
  .Does( () => {
    NpmRunScript( "build", config => config.WorkingDirectory = "../car-reg-ui" );
} );

// TestOnly
Task( "TestOnly" )
  .DoesForEach( GetFiles( "../**/Configit.CarRegistration.API/*.Test/*.csproj" ), (proj) => {
    var resultsPath = new FilePath( $"{Context.Environment.WorkingDirectory}/artifacts/TestResults-{proj.GetFilenameWithoutExtension()}.xml");
    DotNetTest( 
      proj.FullPath, 
      new DotNetCoreTestSettings() {
        Configuration = "Release",
        Logger = $"nunit;LogFilePath={resultsPath}"
      }
    );
  } )
  .DeferOnError();

// UiTestOnly
Task( "UiTestOnly" )
  .Does( () => {
    NpmRunScript( "ciTest", config => config.WorkingDirectory = "../car-reg-ui" );
  } );

// Release
Task( "CreateArtifacts" )
  .Does( () =>
  {
    CopyDirectory( "../car-reg-ui/build", "./artifacts/ui" );
    CopyDirectory( "../Configit.CarRegistration.API/Configit.CarRegistration.API/bin/Release", "./artifacts/backEnd" );
	  
   //Cake Bug Workaround. See: https://github.com/cake-build/cake/issues/2803
   var now = DateTime.UtcNow;
   foreach ( var file in GetFiles( $"./artifacts/**/*" ) ) {
    System.IO.File.SetLastWriteTimeUtc( file.FullPath, now );
   }	  
	  
    Zip( "./artifacts", $"./artifacts/deploy-v{majorVersion}.{minorVersion}.{patchVersion}.zip" );
  } );

//////////////////////////////////////////////////////////////////////
// TASKS WITH DEPENDENCIES
//////////////////////////////////////////////////////////////////////

// Restore solution
Task( "Restore" )
  .IsDependentOn( "EnsureFeeds" )
  .IsDependentOn( "Clean" )
  .IsDependentOn( "RestoreOnly" )
  .Does( () => { } );

// Restore and Build solution
Task( "Build" )
  .IsDependentOn( "Restore" )
  .IsDependentOn( "BuildOnly" )
  .IsDependentOn( "NpmRestoreOnly" )
  .IsDependentOn( "UiBuildOnly" )
  .Does( () => { } );

// Restore, Build and Test solution
Task( "Test" )
  .IsDependentOn( "Build" )
  .IsDependentOn( "TestOnly" )
  .IsDependentOn( "UiTestOnly" )
  .Does( () =>{ } );

// Restore, Build and Test solution
Task( "Release" )
  .IsDependentOn( "Build" )
  .IsDependentOn( "CreateArtifacts" )
  .Does( () =>{ } );

// Run target task
RunTarget( target );
