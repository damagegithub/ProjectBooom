using System.Collections.Generic;
using _ProjectBooom_.Input;
using LYP_Utils.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///     文件可拖拽的半透明UI
    /// </summary>
    public class GhostFileUI : MonoBehaviour
    {
        public RectTransform ParentRectTrans;
        public Canvas        ComputerCanvas;
        public RectTransform CurrentRectTrans;
        public Vector2       MouseOffset;
        public FileUI        SourceFileUI;

        public RectTransform   FileRectTrans;
        public Image           FileImage;
        public TextMeshProUGUI FileNameText;

        public List<FileReceiveUI> FileReceiveUIs = new();

        public Camera MainCamera;

        public void StartDrag(FileUI fileUI)
        {
            SourceFileUI = fileUI;
            FileImage.sprite = fileUI.FileImage.sprite;
            FileNameText.text = fileUI.FileNameText.text;
            gameObject.SetActive(true);

            transform.position = fileUI.transform.position;

            ParentRectTrans.ScreenPointToLocalPointInRectangle(
                InputWarp.MousePosition(),
                ComputerCanvas,
                out Vector2 localPos
            );

            MouseOffset = CurrentRectTrans.anchoredPosition - localPos;
        }

        public void EndDrag()
        {
            SourceFileUI = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (ParentRectTrans.ScreenPointToLocalPointInRectangle(
                    InputWarp.MousePosition(),
                    ComputerCanvas,
                    out Vector2 localPos
                ))
            {
                CurrentRectTrans.anchoredPosition = localPos + MouseOffset;
            }

            if (InputWarp.MouseLeftUp())
            {
                CheckAllFileReceiveUI();
                gameObject.SetActive(false);
            }
        }

        private void CheckAllFileReceiveUI()
        {
            Vector3 mousePosScreen = InputWarp.MousePosition();
            // 检查所有的文件接收处UI
            foreach (FileReceiveUI fileReceiveUI in FileReceiveUIs)
            {
                if (MainCamera 
                  && RectTransformUtility
                       .RectangleContainsScreenPoint(fileReceiveUI.FoldRectTrans, mousePosScreen, MainCamera))
                {
                    fileReceiveUI.ReceiveFile(this);
                    return;
                }
                else if (RectTransformUtility
                        .RectangleContainsScreenPoint(fileReceiveUI.FoldRectTrans, mousePosScreen))
                {
                    fileReceiveUI.ReceiveFile(this);
                    return;
                }
            }
        }
    }
}