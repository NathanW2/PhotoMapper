using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoMapper.CommandLine
{
    public class CommandLineArgs
    {
        private Dictionary<string, string> commandargs = new Dictionary<string, string>();

        public CommandLineArgs(Dictionary<string,string> mappings)
        {
            this.commandargs = mappings;
        }

        public static CommandLineArgs ParseCommandLine(string[] args)
        {
            Dictionary<string, string> mappings = new Dictionary<string, string>();

            foreach (string arg in args)
            {
                // Skip over command line
                if (!arg.StartsWith("/")) continue;

                string[] values = arg.Split(new char[] {':'},2);
                string key = values[0].TrimStart('/');

                // The help key is special just add it to the mappings without a value.
                if (key == "?")
                {
                    mappings.Add(key, "");
                    continue;
                } 

                mappings.Add(key, values[1]);
            }

            return new CommandLineArgs(mappings);
        }

        public string this[string argKey]
        {
            get
            {
                if (this.ContainsArg(argKey))
                {
                    return this.commandargs[argKey];
                }
                else
                    throw new ArgumentOutOfRangeException(String.Format("Arg with key {0} could not be found", argKey)); 
            }
        }

        public bool ContainsArg(string key)
        {
            return this.commandargs.ContainsKey(key);
        }

        public bool HasArgs {
            get
            { 
                return (this.commandargs.Count > 0);
            }
        }

        public delegate T Func<T>();

        public bool IsVaild(Func<bool> vaildcheck)
        {
            return vaildcheck.Invoke();
        }

        public string InDir 
        {
            get
            {
                return this["i"];
            }
        }

        public string OutDir
        {
            get
            {
                return this["outdir"];
            }
        }

        public string OutName
        {
            get
            {
                return this["outname"];
            }
        }

        public PhotoMapper.ImageProcessor.FormatFlags Format
        {
            get
            {
                string format = this["format"];
                switch (format)
                {
                    case "MIF":
                        return ImageProcessor.FormatFlags.MIF;
                    case "TAB":
                        return ImageProcessor.FormatFlags.TAB;
                    case "MIF|TAB":
                        return ImageProcessor.FormatFlags.MIF | ImageProcessor.FormatFlags.TAB;
                    default:
                        throw new NotSupportedException(format + " tag is not supported");
                }
            }
        }
    }
}
