
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WebService1
{
    // This contains logic to load an MPLab 'project' file, and execute pre/post-build tasks.
    public class project
    {
        String projectDir;
        public project(string filename)
        {

            StreamReader projectStream = new StreamReader(filename);
            projectDir = Path.GetDirectoryName(filename);

            String thisLine;
            Regex sectionPattern = new Regex("^\\[.*\\]$"); // Match any line starting with [ and ending with ]
            String currentHeader = "";

            while (null != (thisLine = projectStream.ReadLine()))
            {
                thisLine = thisLine.Trim();

                // If this is a section header, set the currentHeader string
                if (sectionPattern.IsMatch(thisLine))
                    currentHeader = thisLine.Substring(1, thisLine.Length - 2);
                else
                {
                    String[] splot = thisLine.Split(new char[] { '=' }, 2);
                    String lhs = splot[0];
                    String rhs = splot[1];
                    processLine(currentHeader, lhs, rhs);
                }
            }

            if (buildDirPolicy == BuildDirPolicies.BuildDirIsSourceDir)
            {
                dir_src = projectDir;
                dir_bin = projectDir;
                dir_tmp = projectDir;
                dir_sin = projectDir;
                dir_inc = projectDir;
                dir_lib = projectDir;
                dir_lkr = projectDir;
            }
            projectStream.Close();

        }

        public void doPreBuild()
        {
            // exec any pre-build commands
            if (null != preBuild)
            {
                Process preBuildProcess = new Process();
                preBuildProcess.StartInfo.UseShellExecute = false;
                preBuildProcess.StartInfo.FileName = projectDir + "\\" + preBuild;
                // todo: ensure path is correct under different project types
                preBuildProcess.StartInfo.WorkingDirectory = projectDir;
                preBuildProcess.Start();
                if (!preBuildProcess.WaitForExit(int.Parse(ConfigurationManager.AppSettings["preBuildTimeout"])))
                {
                    throw new Exception("timeout waiting for pre-build event");
                }
            }
        }

        public void doPostBuild()
        {
            // exec any post-build commands
            if (null != postBuild)
            {
                Process postBuildProcess = new Process();
                postBuildProcess.StartInfo.UseShellExecute = false;
                postBuildProcess.StartInfo.FileName = projectDir + "\\" + postBuild;
                // todo: ensure path is correct under different project types
                postBuildProcess.StartInfo.WorkingDirectory = projectDir;
                postBuildProcess.Start();
                if (!postBuildProcess.WaitForExit(int.Parse(ConfigurationManager.AppSettings["postBuildTimeout"])))
                {
                    throw new Exception("timeout waiting for post-build event");
                }
            }
        }

        private void processLine(string header, string lhs, string rhs)
        {
            switch (header.ToLower())
            {
                case ("header"):
                    processHeaderLine(lhs, rhs);
                    break;
                case ("path_info"):
                    processPathInfoLine(lhs, rhs);
                    break;
                case ("cat_filters"):
                    processCatFilters(lhs, rhs);
                    break;
                case ("cat_subfolders"):
                    processCatSubFolders(lhs, rhs);
                    break;
                case ("file_subfolders"):
                    processFileSubFolders(lhs, rhs);
                    break;
                case ("generated_files"):
                    processGeneratedFiles(lhs, rhs);
                    break;
                case ("other_files"):
                    processOtherFiles(lhs, rhs);
                    break;
                case ("file_info"):
                    processFileInfo(lhs, rhs);
                    break;
                case ("suite_info"):
                    processSuiteInfo(lhs, rhs);
                    break;
                case ("tool_settings"):
                    processToolSettings(lhs, rhs);
                    break;
                case ("instrumented_trace"):
                    // Don't care about this.
                    break;
                case ("custom_build"):
                    processCustomBuild(lhs, rhs);
                    break;
                default:
                    throw new NotImplementedException("unsupported header " + header);
            }
        }

        private void processCustomBuild(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                // we ignore the buildEnableds.
                case ("pre-buildenabled"):
                    break;
                case ("post-buildenabled"):
                    break;
                case ("pre-build"):
                    preBuild = rhs;
                    break;
                case ("post-build"):
                    postBuild = rhs;
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        private void processToolSettings(string lhs, string rhs)
        {
            switch (lhs.ToUpper())
            {
                case ("TS{DD2213A8-6310-47B1-8376-9430CDFC013F}"):
                    mplink_args = rhs;
                    break;
                case ("TS{BFD27FBA-4A02-4C0E-A5E5-B812F3E7707C}"):
                    mpasm_args = rhs;
                    break;
                case ("TS{ADE93A55-C7C7-4D4D-A4BA-59305F7D0391}"):
                    c17and18_args = rhs;
                    break;

                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        private void processSuiteInfo(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                case ("suite_guid"):
                    if (rhs.ToUpper() == "{6B3DAA78-59C1-46DD-B6AA-DBDAE4E06484}")
                        toolSuite = ToolSuites.mplab;
                    else
                        throw new NotImplementedException("unsupported toolchain with guid " + rhs);
                    break;
                case ("suite_state"):
                    // We silently drop this. No idea what it's used for.
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }

        }

        private void processFileInfo(string lhs, string rhs)
        {
            if (null == projectFiles[lhs])
                projectFiles[lhs] = new projectFile();

            projectFiles[lhs].name = rhs;
        }

        private void processOtherFiles(string lhs, string rhs)
        {
            if (null == projectFiles[lhs])
                projectFiles[lhs] = new projectFile();

            if (rhs.ToLower() == "yes")
                projectFiles[lhs].isOtherFile = true;
            else
                projectFiles[lhs].isOtherFile = false;
        }

        private void processGeneratedFiles(string lhs, string rhs)
        {
            if (null == projectFiles[lhs])
                projectFiles[lhs] = new projectFile();

            if (rhs.ToLower() == "yes")
                projectFiles[lhs].isAutoGenerated = true;
            else
                projectFiles[lhs].isAutoGenerated = false;
        }

        private void processFileSubFolders(string lhs, string rhs)
        {
            if (!projectFiles.ContainsKey(lhs))
                projectFiles.Add(lhs, new projectFile());

            projectFiles[lhs].subfolder = rhs;
        }

        private void processCatSubFolders(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                case ("subfolder_src"):
                    subfolder_src = rhs;
                    break;
                case ("subfolder_inc"):
                    subfolder_inc = rhs;
                    break;
                case ("subfolder_obj"):
                    subfolder_obj = rhs;
                    break;
                case ("subfolder_lib"):
                    subfolder_lib = rhs;
                    break;
                case ("subfolder_lkr"):
                    subfolder_lkr = rhs;
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        private void processCatFilters(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                case ("filter_src"):
                    filter_src = rhs;
                    break;
                case ("filter_inc"):
                    filter_inc = rhs;
                    break;
                case ("filter_obj"):
                    filter_obj = rhs;
                    break;
                case ("filter_lib"):
                    filter_lib = rhs;
                    break;
                case ("filter_lkr"):
                    filter_lkr = rhs;
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        private void processPathInfoLine(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                case ("builddirpolicy"):
                    buildDirPolicy = (BuildDirPolicies)Enum.Parse(typeof(BuildDirPolicies), rhs, true);
                    break;
                case ("dir_src"):
                    dir_src = rhs;
                    break;
                case ("dir_bin"):
                    dir_bin = rhs;
                    break;
                case ("dir_tmp"):
                    dir_tmp = rhs;
                    break;
                case ("dir_sin"):
                    dir_sin = rhs;
                    break;
                case ("dir_inc"):
                    dir_inc = rhs;
                    break;
                case ("dir_lib"):
                    dir_lib = rhs;
                    break;
                case ("dir_lkr"):
                    dir_lkr = rhs;
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        private void processHeaderLine(string lhs, string rhs)
        {
            switch (lhs.ToLower())
            {
                case ("magic_cookie"):
                    magic_cookie = rhs;
                    break;
                case ("file_version"):
                    file_version = float.Parse(rhs);
                    if (file_version > 1.0)
                        throw new NotImplementedException("project file version " + file_version + " is unsupported");
                    break;
                default:
                    throw new NotImplementedException("unsupported project line " + lhs);
            }
        }

        public Single file_version;
        public string magic_cookie;
        public BuildDirPolicies buildDirPolicy;
        public ToolSuites toolSuite;

        public String c17and18_args;
        public String mpasm_args;
        public String mplink_args;

        public string preBuild;
        public string postBuild;

        public string filter_src;
        public string filter_inc;
        public string filter_obj;
        public string filter_lib;
        public string filter_lkr;

        public string dir_src;
        public string dir_bin;
        public string dir_tmp;
        public string dir_sin;
        public string dir_inc;
        public string dir_lib;
        public string dir_lkr;

        public string subfolder_src;
        public string subfolder_obj;
        public string subfolder_inc;
        public string subfolder_lib;
        public string subfolder_lkr;

        public Dictionary<string, projectFile> projectFiles = new Dictionary<string, projectFile>();

    }

    public class projectFile
    {
        public string subfolder;
        public string name;
        public bool isAutoGenerated;
        public bool isOtherFile;
    }

    public enum ToolSuites
    {
        mplab
    }

    public enum BuildDirPolicies
    {
        BuildDirIsSourceDir,
        BuildDirIsProjectDir
    }

}