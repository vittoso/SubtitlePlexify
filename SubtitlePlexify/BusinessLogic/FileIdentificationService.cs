using SubtitlePlexify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubtitlePlexify.Common.Extensions;

namespace SubtitlePlexify.BusinessLogic
{
    public class FileIdentificationService : SubtitlePlexify.BusinessLogic.IFileIdentificationService
    {

        protected static string[] PLEX_SUPPORTED_SUB_EXTENSIONS = { ".srt", ".smi", ".ass", ".ssa" };

        protected static string[] PLEX_SUPPORTED_VIDEO_EXTENSIONS = { ".avi", ".mp4" , ".mkv"}; // Parzialmente popolato

        public void IdentifyFile(ICollection<FileAndSubDTO> list, string newFilePath)
        {
            var ext = Path.GetExtension(newFilePath);
            var filename = Path.GetFileName(newFilePath);

    

            // struttura: parti separate da punti (.)
            char separator = '.';
            int idxFirstSeparator = 0;
            do
            {
                idxFirstSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);
            } while (!StartsWithValidSeasonEpisode(filename, idxFirstSeparator) && idxFirstSeparator != -1); // Migliorare riconoscimento episodio


            // struttura: parti separate da punti (-)
            if (idxFirstSeparator == -1)
            {
                separator = '-';
                idxFirstSeparator = 0;
                do
                {
                    idxFirstSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);
                } while (!StartsWithValidSeasonEpisode(filename, idxFirstSeparator) && idxFirstSeparator != -1); // Migliorare riconoscimento episodio
            }

            int idxSecondSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);


            if (idxSecondSeparator == -1)
                idxSecondSeparator = filename.Length - 1;


            var substringfile = filename.Substring(0, idxSecondSeparator - idxFirstSeparator -1);


            string showName = idxFirstSeparator != -1 ? filename.Substring(0, idxFirstSeparator).Replace(separator, ' ').Trim() :  substringfile;
            string episodeDescription = idxFirstSeparator != -1 && idxSecondSeparator != -1 ? ConvertoToSEFormat(filename.Substring(idxFirstSeparator + 1, idxSecondSeparator - idxFirstSeparator - 1).ToLowerInvariant()) : substringfile;

            if (ext.EndsWithAny(PLEX_SUPPORTED_VIDEO_EXTENSIONS))
            {
               // var file = list.FirstOrDefault(t => t.SubsFile_Path != null && t.SubsFile_Path.ToLowerInvariant().Contains(substringfile.ToLowerInvariant()));
                var file = list.FirstOrDefault(t => t.SubsFile_Path != null && string.Compare(t.ShowName, showName, true) == 0 && string.Compare(t.EpisodeDescription, episodeDescription, true) == 0);

                if (file != null)
                    file.VideoFile_Path = newFilePath;
                else
                    list.Add(new FileAndSubDTO { VideoFile_Path = newFilePath, ShowName = showName, EpisodeDescription = episodeDescription });
            }
            else if (ext.EndsWithAny(PLEX_SUPPORTED_SUB_EXTENSIONS))
            {
               //  var file = list.FirstOrDefault(t => t.VideoFile_Path != null && t.VideoFile_Path.ToLowerInvariant().Contains(substringfile.ToLowerInvariant()));
                var file = list.FirstOrDefault(t => t.VideoFile_Path != null && string.Compare(t.ShowName, showName, true) == 0 && string.Compare(t.EpisodeDescription, episodeDescription, true) == 0);

                if (file != null)
                    file.SubsFile_Path = newFilePath;
                else
                    list.Add(new FileAndSubDTO { SubsFile_Path = newFilePath, ShowName = showName, EpisodeDescription = episodeDescription });
            }
        }


        public void TryProcessNewSubsPath(FileAndSubDTO item, string lang = "it")
        {
            if (item.VideoFile_Path != null
                    && item.SubsFile_Path != null
                    && File.Exists(item.VideoFile_Path)
                    && File.Exists(item.SubsFile_Path)
                    )
            {
                string dir = Path.GetDirectoryName(item.VideoFile_Path);
                string fileWE = Path.GetFileNameWithoutExtension(item.VideoFile_Path);
                string subext = Path.GetExtension(item.SubsFile_Path);
                item.SubsFileRename_Path = Path.Combine(dir, string.Format("{0}.{1}{2}", fileWE, lang, subext));
            }
        }


        #region Metodi privanti

        private static bool StartsWithValidSeasonEpisode(string filename, int idxSeparator)
        {
            var substring = filename.Substring(idxSeparator + 1).Trim().ToLowerInvariant();

            //struttura: sNNeNN

            if (substring.StartsWith("s")
                && (substring.Length > 3 && substring[3] == 'e')
                && (char.IsDigit(substring[1]))
                && (char.IsDigit(substring[2])))
                return true;

            // struttura [NNxNN]

            if (substring.StartsWith("[")
                && (substring.Length > 6 && substring[6] == ']')
                && (substring.Length > 3 && substring[3] == 'x')
                && (char.IsDigit(substring[1]))
                && (char.IsDigit(substring[2])))
                return true;


            // struttura NNxNN

            if ((substring.Length > 2 && substring[2] == 'x')
                && (char.IsDigit(substring[0]))
                && (char.IsDigit(substring[1])))
                return true;

            return false;
        }

        /// <summary>
        /// Converte in formato sNNeNN
        /// </summary>
        /// <param name="episodeDescription"></param>
        /// <returns></returns>
        protected static string ConvertoToSEFormat(string episodeDescription)
        {
            string substring = episodeDescription.Trim().ToLowerInvariant();

            //struttura: sNNeNN
            if (substring.StartsWith("s")
              && (substring.Length > 3 && substring[3] == 'e')
              && (char.IsDigit(substring[1]))
              && (char.IsDigit(substring[2])))
                return substring;

            // struttura [NNxNN]

            if (substring.StartsWith("[")
                && (substring.Length > 6 && substring[6] == ']')
                && (substring.Length > 3 && substring[3] == 'x')
                && (char.IsDigit(substring[1]))
                && (char.IsDigit(substring[2]))
                && (char.IsDigit(substring[4]))
                && (char.IsDigit(substring[5])))
            {
                return string.Format("s{0}{1}e{2}{3}", substring[1] ,substring[2], substring[4], substring[5]);
            }


            // struttura NNxNN

            if ((substring.Length > 2 && substring[2] == 'x')
                && (char.IsDigit(substring[0]))
                && (char.IsDigit(substring[1]))
                && (char.IsDigit(substring[3]))
                && (char.IsDigit(substring[4])))
            {
                return string.Format("s{0}{1}e{2}{3}", substring[0], substring[1], substring[3], substring[4]);
            }

            // Formato non riconosciuto
            return episodeDescription;
        }
        #endregion
    }
}
