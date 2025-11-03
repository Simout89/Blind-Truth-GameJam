using System;
using FMODUnity;
using TMPro;
using UnityEngine;
using Zenject;
using Скриптерсы.Datas;

namespace Скриптерсы.View
{
    public class NoteView: MonoBehaviour
    {
        [SerializeField] private GameObject note;
        [SerializeField] private TMP_Text _text;
        [Inject] private GameStateManager _gameStateManager;
        private NoteData lastData;

        private void Awake()
        {
            Hide();
        }

        public void TryShowNote(NoteData noteData)
        {
            note.SetActive(true);
            _text.text = noteData.text;
            _gameStateManager.ChangeState(GameStates.Note);
            lastData = noteData;
        }

        public void Hide()
        {
            note.SetActive(false);
            _gameStateManager.ChangeState(GameStates.Play);
            if(lastData.soundAfterClose != "")
                RuntimeManager.PlayOneShot(lastData.soundAfterClose);
        }
    }
}