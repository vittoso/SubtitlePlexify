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

        protected static string[] PLEX_SUPPORTED_VIDEO_EXTENSIONS = { ".avi", ".mp4", ".mkv" }; // Parzialmente popolato

        public void IdentifyFile(ICollection<FileAndSubDTO> list, string newFilePath)
        {
            var ext = Path.GetExtension(newFilePath);
            var filename = Path.GetFileName(newFilePath);


            if (!ext.EndsWithAny(PLEX_SUPPORTED_VIDEO_EXTENSIONS) && !ext.EndsWithAny(PLEX_SUPPORTED_SUB_EXTENSIONS))
                return; // unidentified file type (neither VIDEO or SUBS)


            string showName, episodeDescription;
            bool result = TryIdentifyWithDot(filename, out showName, out episodeDescription);

            if (!result)
                result = TryIdentifyWithMinus(filename, out showName, out episodeDescription);

            if (!result)
                result = TryIdentifyWithUnderscore(filename, out showName, out episodeDescription);

            if (result)
            {
                if (ext.EndsWithAny(PLEX_SUPPORTED_VIDEO_EXTENSIONS))
                {
                    var file = list.FirstOrDefault(t =>
                        t.SubsFile_Path != null && string.Compare(t.ShowName, showName, true) == 0 &&
                        string.Compare(t.EpisodeDescription, episodeDescription, true) == 0);

                    if (file != null)
                        file.VideoFile_Path = newFilePath;
                    else
                        list.Add(new FileAndSubDTO
                        {
                            VideoFile_Path = newFilePath,
                            ShowName = showName,
                            EpisodeDescription = episodeDescription
                        });
                }
                else if (ext.EndsWithAny(PLEX_SUPPORTED_SUB_EXTENSIONS))
                {
                    var file = list.FirstOrDefault(t =>
                        t.VideoFile_Path != null && string.Compare(t.ShowName, showName, true) == 0 &&
                        string.Compare(t.EpisodeDescription, episodeDescription, true) == 0);

                    if (file != null)
                        file.SubsFile_Path = newFilePath;
                    else
                        list.Add(new FileAndSubDTO
                        {
                            SubsFile_Path = newFilePath,
                            ShowName = showName,
                            EpisodeDescription = episodeDescription
                        });
                }
            }
            else
            {
                if (ext.EndsWithAny(PLEX_SUPPORTED_VIDEO_EXTENSIONS))
                {
                    list.Add(new FileAndSubDTO
                    {
                        VideoFile_Path = newFilePath,
                        ShowName = showName,
                        EpisodeDescription = episodeDescription
                    });
                }
                else if (ext.EndsWithAny(PLEX_SUPPORTED_SUB_EXTENSIONS))
                {
                    list.Add(new FileAndSubDTO
                    {
                        SubsFile_Path = newFilePath,
                        ShowName = showName,
                        EpisodeDescription = episodeDescription
                    });
                }
            }
        }

        protected static bool TryIdentifyWithDot(string filename, out string showName, out string episodeDescription)
        {
            bool result = false;
            showName = filename;
            episodeDescription = filename;
            const char separator = '.';
            int idxFirstSeparator = 0;
            do
            {
                idxFirstSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);
            } while (!StartsWithValidSeasonEpisode(filename, idxFirstSeparator) && idxFirstSeparator != -1
            );


            if (idxFirstSeparator == -1)
                return result;


            int idxSecondSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);


            if (idxSecondSeparator == -1)
                idxSecondSeparator = filename.Length - 1;


            var substringfile = filename.Substring(0, idxSecondSeparator - idxFirstSeparator - 1);


            showName = idxFirstSeparator != -1
                ? ConvertShowName_To_Standard_Format(filename.Substring(0, idxFirstSeparator).Replace(separator, ' ').Trim())
                : substringfile;
            episodeDescription = idxFirstSeparator != -1 && idxSecondSeparator != -1
                ? ConvertEpisodeDescription_To_sNNeNN_Format(filename.Substring(idxFirstSeparator + 1, idxSecondSeparator - idxFirstSeparator - 1)
                    .ToLowerInvariant())
                : substringfile;
            result = true;
            return result;
        }

        protected static bool TryIdentifyWithMinus(string filename, out string showName, out string episodeDescription)
        {
            bool result = false;
            showName = filename;
            episodeDescription = filename;
            const char separator = '-';
            int idxFirstSeparator = 0;
            do
            {
                idxFirstSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);
            } while (!StartsWithValidSeasonEpisode(filename, idxFirstSeparator) && idxFirstSeparator != -1
            );


            if (idxFirstSeparator == -1)
                return result;


            int idxSecondSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);


            if (idxSecondSeparator == -1)
                idxSecondSeparator = filename.Length - 1;


            var substringfile = filename.Substring(0, idxSecondSeparator - idxFirstSeparator - 1);


            showName = idxFirstSeparator != -1
                ? ConvertShowName_To_Standard_Format(filename.Substring(0, idxFirstSeparator).Replace(separator, ' ').Trim())
                : substringfile;
            episodeDescription = idxFirstSeparator != -1 && idxSecondSeparator != -1
                ? ConvertEpisodeDescription_To_sNNeNN_Format(filename.Substring(idxFirstSeparator + 1, idxSecondSeparator - idxFirstSeparator - 1)
                    .ToLowerInvariant())
                : substringfile;
            result = true;
            return result;
        }

        protected static bool TryIdentifyWithUnderscore(string filename, out string showName, out string episodeDescription)
        {
            bool result = false;
            showName = filename;
            episodeDescription = filename;
            const char separator = '_';
            int idxFirstSeparator = 0;
            do
            {
                idxFirstSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);
            } while (!StartsWithValidSeasonEpisode(filename, idxFirstSeparator) && idxFirstSeparator != -1
            );


            if (idxFirstSeparator == -1)
                return result;


            int idxSecondSeparator = filename.IndexOf(separator, idxFirstSeparator + 1);


            if (idxSecondSeparator == -1)
                idxSecondSeparator = filename.Length - 1;


            var substringfile = filename.Substring(0, idxSecondSeparator - idxFirstSeparator - 1);


            showName = idxFirstSeparator != -1
                ? ConvertShowName_To_Standard_Format(filename.Substring(0, idxFirstSeparator).Replace(separator, ' ').Trim())
                : substringfile;
            episodeDescription = idxFirstSeparator != -1 && idxSecondSeparator != -1
                ? ConvertEpisodeDescription_To_sNNeNN_Format(filename.Substring(idxFirstSeparator + 1, idxSecondSeparator - idxFirstSeparator - 1)
                    .ToLowerInvariant())
                : substringfile;
            result = true;
            return result;
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


        #region Metodi privati

        protected static bool StartsWithValidSeasonEpisode(string filename, int idxSeparator)
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
        protected static string ConvertEpisodeDescription_To_sNNeNN_Format(string episodeDescription)
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
                return string.Format("s{0}{1}e{2}{3}", substring[1], substring[2], substring[4], substring[5]);
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

        protected static string ConvertShowName_To_Standard_Format(string showName)
        {
            if (string.IsNullOrWhiteSpace(showName))
                return null;


            var returnShowName = showName.Replace(".", " ");

            //There are no spaces in name but there are multiple single CAPS letters... try to separate by capital
            // TheBestSeries => The Best Series
            // but be careful!
            // NCISTheBestSeries => NCIS The Best Series
            if (!returnShowName.Contains(' ') && char.IsUpper(returnShowName[0]) && returnShowName.Count(c => char.IsUpper(c)) >= 2)
            {
                StringBuilder dest = new StringBuilder();
                for (int i = 0; i < returnShowName.Length; i++)
                {
                    char c = returnShowName[i];
                    if (char.IsUpper(c))
                    {
                        if (i == 0) // initial char
                            dest.Append(c);
                        else if (char.IsUpper(returnShowName[i - 1]) && (i < returnShowName.Length - 1) && char.IsUpper(returnShowName[i + 1]))
                            dest.Append(c);
                        else
                            dest.Append(' ').Append(c);
                    }
                    else
                        dest.Append(c);
                }
                returnShowName = dest.ToString();

            }
            return returnShowName;

        }
        #endregion
    }
}
