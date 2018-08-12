using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubtitlePlexify.BusinessLogic;
using System.Collections.Generic;
using SubtitlePlexify.Model;

namespace SubtitlePlexify.Test
{
    [TestClass]
    public class FileIdentificationServiceTest
    {
        FileIdentificationService service;
        [TestInitialize]
        public void TestInitialize()
        {
            service = new FileIdentificationService();
        }

        [TestMethod]
        public void TwoFiles_StandardNamesMp4Srt_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.S21E01.uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.S21E01.fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }


        [TestMethod]
        public void TwoFiles_StandardNamesMp4Srt_NotCaseSensitive_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.s21E01.uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.S21e01.fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_StandardNamesMp4Srt_SMI_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.S21E01.uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.S21E01.fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.smi";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_StandardNamesMp4Srt_Ass_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.S21E01.uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.S21E01.fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.ass";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_StandardNamesMp4Srt_Ssa_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.S21E01.uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.S21E01.fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.ssa";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_BracketNamesMp4Srt_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.[21x01].uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.[21x01].fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_BracketNamesMp4Srt_CaseInsensitive_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.[21x01].uhgjfgsdhgferhgr8743384756923874yughhjrstret.mp4";
            string srtFile = "The.Series.[21X01].fv5342t78yt78yrhsgfbvhskerkjhfgb8iq3.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s21e01", list[0].EpisodeDescription);
        }


        [TestMethod]
        public void TwoFiles_BracketNamesAviSrt_MinusSeparator_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series - [18x01] - dfgdsfkghsdfghsdfuihgisdfuhg98tys8erygsiduhgsdjkgh.avi";
            string srtFile = "The.Series.s18e01.fgusyfgutysir4y5iw38456w83465.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s18e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_BracketNamesAviSrt_MinusSeparator_NothingAfterEpisodesSpecification_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series - [16x01].avi";
            string srtFile = "The.Series.s16E01.365gerg2343faeregaergfgha456a345.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s16e01", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_NoBracketNamesMkvSrt_MinusSeparator_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series.S15E23.HDTV.fsdfsdfsdfsdfsdf.mkv";
            string srtFile = "The Series - 15x23 - adfgadfgadhafghartyhath - Sub.Ita.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s15e23", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_UnderscoreSeparator_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series.S15E23.ikgerierfgiksrjgrgaergg.mkv";
            string srtFile = "The Series_15x23_tsertgser5tys456w4.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s15e23", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_CompositeNamesDifferentByDot_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series.S15E23.4fsrgzdtstrhstrhstrh.avi";
            string srtFile = "The.Series_15x23_fgzdfgzdsatrhs456w45.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s15e23", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_CompositeNamesDifferentBySpaces_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The Series.S15E23.H123456tfcftfforfsr.mkv";
            string srtFile = "TheSeries_15x23_rgzdfgdrgzdfgxrt.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s15e23", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_CompositeNamesDifferentByDotAndSpaces_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "The.Series.S04E04.fgsfghsfghsgjdgyjsthstrae4taehae5haz.mkv";
            string srtFile = "TheSeries_s04e04_adfvayuerfquk34y5qo834yq934ta.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("The Series", list[0].ShowName);
            Assert.AreEqual("s04e04", list[0].EpisodeDescription);
        }

        [TestMethod]
        public void TwoFiles_CompositeNamesWithCapitalDifferentBySpaces_OK()
        {
            List<FileAndSubDTO> list = new List<FileAndSubDTO>();

            string videoFile = "FBI The Series.S15E23.H123456tfcftfforfsr.mkv";
            string srtFile = "FBITheSeries_15x23_rgzdfgdrgzdfgxrt.srt";
            service.IdentifyFile(list, videoFile);
            service.IdentifyFile(list, srtFile);

            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual(videoFile, list[0].VideoFile_Path);
            Assert.AreEqual(srtFile, list[0].SubsFile_Path);
            Assert.AreEqual("FBI The Series", list[0].ShowName);
            Assert.AreEqual("s15e23", list[0].EpisodeDescription);
        }
    }
}
