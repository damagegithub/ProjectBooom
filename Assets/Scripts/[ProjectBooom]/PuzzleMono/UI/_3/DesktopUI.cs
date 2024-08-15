using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///  桌面UI读取
    /// </summary>
    public class DesktopUI : MonoBehaviour
    {
        public Transform  ComputeUI;
        public Transform  RecycleUI;
        public Transform  DesktopUIRoot;
        public int        MaxCustomUI = 14;
        public GameObject CustomUIPrefab;

        public void DestroyWithoutGameUI()
        {
            for (int i = DesktopUIRoot.childCount - 1; i >= 0; i--)
            {
                Transform child = DesktopUIRoot.GetChild(i);
                if (child.Equals(ComputeUI) || child.Equals(RecycleUI))
                {
                    continue;
                }

                DestroyImmediate(child.gameObject);
            }
        }


        private void Awake()
        {
            DestroyWithoutGameUI();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string[] desktopFileNames = Directory.EnumerateFileSystemEntries(desktopPath)
                                                 .Take(MaxCustomUI)
                                                 .ToArray();
            foreach (string desktopFileName in desktopFileNames)
            {
                string pureFileName = Path.GetFileNameWithoutExtension(desktopFileName);
                GameObject customUI = Instantiate(CustomUIPrefab, DesktopUIRoot);
                customUI.name = $"file_{pureFileName}";
                FileUI fileUI = customUI.GetComponent<FileUI>();
                fileUI.FileImage.enabled = false;
                fileUI.FileNameText.SetText(pureFileName);

                Image[] images = customUI.GetComponentsInChildren<Image>(true);
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].enabled = false;
                    images[i].raycastTarget = false;
                }
            }

            ComputeUI.SetSiblingIndex(0);
            RecycleUI.SetSiblingIndex(1);
        }
    }
}