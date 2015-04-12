using System;
using System.Collections.Generic;
using SubtitlePlexify.Model;
namespace SubtitlePlexify.BusinessLogic
{
    public interface IFileIdentificationService
    {
        /// <summary>
        /// Data una string prova ad inserirla nella lista corrente di di FileAndSubDTO facendo eventualmente matching.
        /// Se viene trovato un match con un file già esistente, modifica l'item di tipo FileAndSubDTO corrispondente
        /// Altrimenti il file viene semplicemente aggiunto alla lista, crando un nuovo item FileAndSubDTO
        /// </summary>
        /// <param name="list"></param>
        /// <param name="newFilePath"></param>
        void IdentifyFile(ICollection<FileAndSubDTO> list, string newFilePath);

        /// <summary>
        /// Dato un item FileAndSubDTO, tenta di generare un nuovo path per il file di sottotitolo rinominato, usando impostazioni standard
        /// </summary>
        /// <param name="item"></param>
        void TryProcessNewSubsPath(FileAndSubDTO item, string lang = "it");
    }
}
