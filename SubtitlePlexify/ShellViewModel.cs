using GongSolutions.Wpf.DragDrop;
using SubtitlePlexify.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.IO;
using SubtitlePlexify.BusinessLogic;
using System.Threading.Tasks;
using System.Threading;
namespace SubtitlePlexify
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell, IDropTarget
    {

        IFileIdentificationService fileIdentificationService;

        public ShellViewModel()
        {
            fileIdentificationService = new FileIdentificationService();
        }

        static string LANG = "it";

        public void DragOver(IDropInfo dropInfo)
        {
            var dropPossible = dropInfo.Data != null && ((DataObject)dropInfo.Data).ContainsFileDropList();
            if (dropPossible)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }


        }

        public void Drop(IDropInfo dropInfo)
        {
            ShellIsBusy = true;
            if (dropInfo.Data is DataObject && ((DataObject)dropInfo.Data).ContainsFileDropList())
            {
                ObservableCollection<FileAndSubDTO> list = new ObservableCollection<FileAndSubDTO>();
                foreach (string filePath in ((DataObject)dropInfo.Data).GetFileDropList())
                {
                    var attributes = File.GetAttributes(filePath);
                    bool isDir = (attributes & FileAttributes.Directory) == FileAttributes.Directory;

                    if (isDir)
                    {
                        foreach (var item in Directory.EnumerateFiles(filePath))
                        {
                            ProcessDroppedFile(list, item);
                        }
                    }
                    else
                        ProcessDroppedFile(list, filePath);
                }

                foreach (var item in list)
                {
                    fileIdentificationService.TryProcessNewSubsPath(item);
                }
                FileMatchList = list;
            }
            ShellIsBusy = false;
        }

        private void ProcessDroppedFile(ObservableCollection<FileAndSubDTO> list, string filePath)
        {
            fileIdentificationService.IdentifyFile(list, filePath);
        }



        private ObservableCollection<FileAndSubDTO> fileMatchListVar = new ObservableCollection<FileAndSubDTO>();

        public ObservableCollection<FileAndSubDTO> FileMatchList
        {
            get { return fileMatchListVar; }
            set
            {
                fileMatchListVar = value;

                NotifyOfPropertyChange(() => this.FileMatchList);
                NotifyOfPropertyChange(() => CanElabora);
                NotifyOfPropertyChange(() => CanRinomina);
                NotifyOfPropertyChange(() => CanSvuota);
            }
        }


        public bool CanElabora
        {
            get { return FileMatchList != null && FileMatchList.Any(); }
        }

        public void Elabora()
        {
            ShellIsBusy = true;
            var list = FileMatchList;

            list.OrderBy(p => p.VideoFile_Path);

            foreach (var item in list)
            {
                fileIdentificationService.TryProcessNewSubsPath(item, LANG);
            }

            FileMatchList = new ObservableCollection<FileAndSubDTO>(list);
            ShellIsBusy = false;
        }

        public bool CanRinomina
        {
            get { return FileMatchList != null && FileMatchList.Any(x => x.CanRinomina); }
        }

        public async  void Rinomina()
        {
            ShellIsBusy = true;
            Task t = Task.Run(() =>
                   {
                       foreach (var item in FileMatchList)
                       {
                           if (item.CanRinomina)
                               File.Move(item.SubsFile_Path, item.SubsFileRename_Path);
                       }
                   });
            await t;
            ShellIsBusy = false;
        }


        public bool CanSvuota
        {
            get { return FileMatchList != null && FileMatchList.Any(); }
        }

        public void Svuota()
        {
            FileMatchList = new ObservableCollection<FileAndSubDTO>();

        }

        private bool shellIsBusy = false;
        public bool ShellIsBusy
        {
            get { return shellIsBusy; }
            set
            {
                if (shellIsBusy != value)
                {
                    shellIsBusy = value;
                    NotifyOfPropertyChange(() => this.ShellIsBusy);
                }
            }
        }
    }
}