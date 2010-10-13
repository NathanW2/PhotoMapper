using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace PhotoMapper.Core
{
    public static class About
    {
        public static string AboutString
        {
            get
            {
                return
                    @"PhotoMapper Utility by Nathan Woodrow
http://code.google.com/p/nathansmapinfoprojects/
http://woostuff.wordpress.com/

Allows processing of images that contain coordinates. 
Can output to MapInfo TAB and/or MID/MIF. 
At the moment it needs MapInfo Pro installed to create a TAB file,
however MapInfo is not needed if you are just creating a MID/MIF. 

PhotoMapper uses Microsoft VirtualEarth to allow for quick visualization of
photo locations at process time.

Use PhotoMapper.Cmd for batch/command line processing.

Change Set
Version 1.5.1.0
- Folder and File picker for input now remember last location.
- Can sync output path with input path.
- Added background logging.
- Command line now supports updating image GPS info.
    - See help file section 'Updating GPS Info' for details
- Command line can output list of files with no GPS info, can be used to update images.";
            }
        }
    }
}
