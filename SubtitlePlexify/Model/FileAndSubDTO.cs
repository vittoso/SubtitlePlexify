using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlePlexify.Model
{
    public class FileAndSubDTO
    {
        public string ShowName { get; set; }
        public string EpisodeDescription { get; set; }

        public string VideoFile_Path { get; set; }
        public string SubsFile_Path { get; set; }

        public string SubsFileRename_Path { get; set; }

        public bool CanRinomina
        {
            get
            {
                return VideoFile_Path != null && SubsFile_Path != null && SubsFileRename_Path != null;
            }
        }
    }
}
