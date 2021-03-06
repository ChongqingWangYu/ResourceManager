﻿using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FromMain : Form
    {
        FromMainBLL bll;
         //主窗口构造方法
         public FromMain()
        {
            //主窗控件初始化
            InitializeComponent();
        }

        //窗口加载事件
        private void fromMain_Load(object sender, EventArgs e)
        {
            //bll = new FromMainBLL(treeView, listView, curPathText, leftPathButton, rightPathButton, backUpPathButton, fileCheckBox, folderCheckBox, this.Handle, largeIconImageList, smallImageList);
            bll = new FromMainBLL();
            bll.setTreeView(treeView).setListView(listView).setCurPathText(curPathText).setLeftPathButton(leftPathButton)
                .setRightPathButton(rightPathButton).setBackUpPathButton(backUpPathButton).setFileCheckBox(fileCheckBox)
                .setFolderCheckBox(folderCheckBox).setHandle(this.Handle).setLargeIconImageList(largeIconImageList)
                .setSmallImageList(smallImageList).setFileCountText(fileCountText).setContextMenuStrip(contextMenuStrip);
            bll.fromMainInit();
        }
        //树节点点击事件
        private void treeViewNodeAfterSelect(object sender, TreeViewEventArgs e)
        {
            bll.addPath = true;
            //树节点显示
            bll.treeViewShow(e.Node);
            //列表显示
            bll.listViewShow(e.Node);
            bll.updatePathButtonState();
        }

        //树节点展开事件
        private void treeViewNodeBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //树节点显示
            bll.treeViewShowFolderTwoTier(e.Node);
        }
        //列表文件夹双击事件
        private void listViewFileDoubleClick(object sender, EventArgs e)
        {
            bll.addPath = true;
            foreach (int ListIndex in listView.SelectedIndices)
            {
                bll.listViewShow(bll.curPath+listView.Items[ListIndex].Text);
            }
        }
        
        //left目录
        private void leftPath_Click(object sender, EventArgs e)
        {
            bll.leftReturnPath();
        }
        //rigth目录
        private void rightPath_Click(object sender, EventArgs e)
        {
            bll.rightReturnPath();
        }
        //回到上一级目录
        private void backUp_Click(object sender, EventArgs e)
        {
            bll.backUpPath();
        }

        private void curPathText_KeyDown(object sender, KeyEventArgs e)
        {
            //判断回车键
            if (e.KeyCode == Keys.Enter)
            {
                bll.listViewShow(curPathText.Text);
            }
        }

        private void largeIconShowMenuItem_Click(object sender, EventArgs e)
        {
            bll.largeIconShow();
        }

        private void smallIconShowMenuItem_Click(object sender, EventArgs e)
        {
            bll.smallIconShow();
        }

        private void detailsShowMenuItem_Click(object sender, EventArgs e)
        {
            bll.detailsShow();
        }

        private void listShowMenuItem_Click(object sender, EventArgs e)
        {
            bll.listShow();
        }

        private void folderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bll.showFilter();
        }

        private void fileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bll.showFilter();
        }

        private void renameMenuItem_Click(object sender, EventArgs e)
        {
            string oldPath=null;
            string newFileName=null;
            string oldFileName = null;
            //获取选择的文件路径
            foreach (int ListIndex in listView.SelectedIndices)
            {
                oldPath=bll.curPath + listView.Items[ListIndex].Text;
            }
            //获取原文件名
            oldFileName = bll.getFileName(oldPath);
            if (oldFileName == null)
            {
                return;
            }
            //弹出框
            UI.FileNameInputBox fileNameInputBox = new FileNameInputBox();
            //设置原文件名
            fileNameInputBox.oldFileName = oldFileName;
            fileNameInputBox.ShowDialog();
            //如果弹出框返回OK
            if (fileNameInputBox.DialogResult == DialogResult.OK)
            {
                //获得新文件名
                newFileName = fileNameInputBox.newFileName;
            }
            //修改文件名
            if (oldPath != null && newFileName != null)
            {
                bll.rename(oldPath, newFileName);
            }
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            //获取选择的文件路径
            System.Collections.IList list = listView.SelectedIndices;
                bll.deleteBatches(list);
        }

        private void refreshMenuItem_Click(object sender, EventArgs e)
        {
            bll.refreshList();
        }

        private void newFileMenuItem_Click(object sender, EventArgs e)
        {
            //弹出框
            UI.FileNameInputBox fileNameInputBox = new FileNameInputBox();
            fileNameInputBox.ShowDialog();
            //如果弹出框返回OK
            if (fileNameInputBox.DialogResult == DialogResult.OK)
            {
                //创建文件
                bll.createFile(fileNameInputBox.newFileName);
            }
        }

        private void newFolderMenuItem_Click(object sender, EventArgs e)
        {
            //弹出框
            UI.FileNameInputBox fileNameInputBox = new FileNameInputBox();
            fileNameInputBox.oldFileName="新建文件夹";
            fileNameInputBox.ShowDialog();
            //如果弹出框返回OK
            if (fileNameInputBox.DialogResult == DialogResult.OK)
            {
                //创建文件夹
                bll.createFolder(fileNameInputBox.newFileName);
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fileName = null;
            //获取选择的文件路径
            foreach (int ListIndex in listView.SelectedIndices)
            {
                //加上文件名
                fileName = listView.Items[ListIndex].Text;
            }
            bll.updateContextMenuStrip(fileName);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            //获取选择的文件路径
            foreach (int ListIndex in listView.SelectedIndices)
            {
                //打开文件
                bll.open(bll.curPath+ listView.Items[ListIndex].Text);
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            //判断回车键
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Console.WriteLine("打开");
                    openMenuItem_Click(null, null);
                    break;
                case Keys.Delete:
                    Console.WriteLine("删除");
                    deleteMenuItem_Click(null, null);
                    break;
                case Keys.Back:
                    Console.WriteLine("后退");
                    leftPath_Click(null, null);
                    break;
                default:
                    break;
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Console.WriteLine("复制");
                        copyMenuItem_Click(null, null);
                        break;
                    case Keys.V:
                        Console.WriteLine("粘贴");
                        pasteMenuItem_Click(null, null);
                        break;
                    case Keys.X:
                        Console.WriteLine("剪切");
                        moveToMenuItem_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            bll.isMoveTo = false;
            foreach (int ListIndex in listView.SelectedIndices)
            {
                bll.copy(bll.curPath + listView.Items[ListIndex].Text);
            }
        }

        private void pasteMenuItem_Click(object sender, EventArgs e)
        {
            string path = bll.curPath;
            foreach (int ListIndex in listView.SelectedIndices)
            {
                path += listView.Items[ListIndex].Text;
            }
            bll.paste(path);
        }

        private void moveToMenuItem_Click(object sender, EventArgs e)
        {
            bll.isMoveTo = true;
            foreach (int ListIndex in listView.SelectedIndices)
            {
                bll.moveTo(bll.curPath + listView.Items[ListIndex].Text);
            }
        }
    }
}