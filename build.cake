#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

#addin nuget:?package=SharpZipLib&version=1.1.0
#addin nuget:?package=Cake.Compression&version=0.2.3

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var platformArg = Argument("platformArg", "x64");

Information("Platform argument is: {0}", platformArg);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var assemblyInfo = ParseAssemblyInfo("./src/SharedAssemblyInfo.cs");
var appVersion = assemblyInfo.AssemblyVersion;
Information("Version: {0}", appVersion);

var platformTarget = PlatformTarget.MSIL;
var platformName = string.Empty;
switch (platformArg) 
{
	case "x86":
		platformTarget = PlatformTarget.x86;
		platformName = "win32";
		break;
	default:
		platformTarget = PlatformTarget.x64;
		platformName = "win64";
		break;
}
Information("Platform name: {0}", platformName);

var buildDir = Directory("./src/AMI.Portable/bin") + Directory(platformArg) + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Download-Dependencies")
	.IsDependentOn("Clean")
    .Does(() =>
{
	var simpleITK_dir = string.Empty;
	var simpleITK_zip = string.Empty;
	var simpleITK_url = string.Empty;

	switch (platformArg) 
	{
		case "x86":
			simpleITK_dir = "./libs/SimpleITK-1.2.0-CSharp-win32-x86";
			simpleITK_zip = "./libs/SimpleITK-1.2.0-CSharp-win32-x86.zip";
			simpleITK_url = "https://github.com/SimpleITK/SimpleITK/releases/download/v1.2.0/SimpleITK-1.2.0-CSharp-win32-x86.zip";
			break;
		default:
			simpleITK_dir = "./libs/SimpleITK-1.2.0-CSharp-win64-x64";
			simpleITK_zip = "./libs/SimpleITK-1.2.0-CSharp-win64-x64.zip";
			simpleITK_url = "https://github.com/SimpleITK/SimpleITK/releases/download/v1.2.0/SimpleITK-1.2.0-CSharp-win64-x64.zip";
			break;
	}

	if (!DirectoryExists(simpleITK_dir))
	{
		if (!FileExists(simpleITK_zip))
		{
			// Downloading SimpleITK
			var outputPath = File(simpleITK_zip);
			DownloadFile(simpleITK_url, outputPath);
		}
		// Unzipping SimpleITK
		Unzip(simpleITK_zip, "./libs");
	}
	var simpleITK_file1 = "SimpleITKCSharpManaged.dll";
	var simpleITK_file2 = "SimpleITKCSharpNative.dll";
	CopyFile(Directory(simpleITK_dir) + File(simpleITK_file1), Directory("./libs") + File(simpleITK_file1));
	CopyFile(Directory(simpleITK_dir) + File(simpleITK_file2), Directory("./libs") + File(simpleITK_file2));
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Download-Dependencies")
    .Does(() =>
{
    NuGetRestore("./src/AMI.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/AMI.sln", settings =>
        settings.SetConfiguration(configuration).SetPlatformTarget(platformTarget));
    }
    else
    {
      // Use XBuild
      XBuild("./src/AMI.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Zip-Build")
	.IsDependentOn("Build")
    .Does(() =>
{
	Zip(buildDir, buildDir + File("AMI-Portable-" + appVersion + "-" + platformName + "-" + platformArg + ".zip"));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Zip-Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
