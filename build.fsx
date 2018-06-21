#r "paket: groupref Build //"
#load "./.fake/build.fsx/intellisense.fsx"
#if !FAKE
  #r "netstandard"
#endif

#load "MonoGameContent.fsx"

open Fake.Core
open Fake.IO.Globbing.Operators

// Directories

let intermediateContentDir = "./intermediateContent/"
let contentDir = "./src/MonoGame/"
let buildDir  = "./build/"
let deployDir = "./deploy/"

// Filesets

let appReferences = 
    !! "**/*.fsproj"

let contentFiles =
    !! "**/*.fx"
        ++ "**/*.spritefont"
        ++ "**/*.dds"

// Targets

Target.create "Clean" (fun _ -> 
    Fake.IO.Shell.cleanDirs [buildDir; deployDir]
)

Target.create "BuildContent" (fun _ ->
    contentFiles
        |> MonoGameContent.buildMonoGameContent (fun p ->
            { p with
                OutputDir = contentDir;
                IntermediateDir = intermediateContentDir;
            }))

Target.create "BuildApp" (fun _ ->
    appReferences
        |> Fake.DotNet.MSBuild.runDebug id buildDir "Build"
        |> ignore
)

Target.create "RunApp" (fun _ ->
    Fake.Core.Process.fireAndForget (fun info ->
        { info with
            FileName = buildDir + @"fsharp-MonoGame.exe"
            WorkingDirectory = buildDir })
    Fake.Core.Process.setKillCreatedProcesses false)

// Build order

open Fake.Core.TargetOperators

"Clean"
    ==> "BuildContent"
    ==> "BuildApp"
    ==> "RunApp"

// Start build

Target.runOrDefault "BuildApp"