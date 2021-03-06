﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Notepad.Services;
using Notepad.Dtos;
using Notepad.TODO.Tests;

namespace Notepad.UI
{
    public class IdeaController
    {
        private readonly TabControl _area;
        private IdeaService _ideaService;
        private readonly ILoggingController _loggingController;

        public void SetFont(Font font)
        {
            GetIdeaList(_area).Font = font;
        } 

        public void SetBackColor(Color color)
        {
            GetIdeaList(_area).BackColor = color;
        }

        public void SetForeColor(Color color)
        {
            GetIdeaList(_area).ForeColor = color;
        }


        private void PopulateData(ListView ideaList)
        {
      
         var ideas=_ideaService.All();
          foreach (var idea in ideas)
          {
              ideaList.Items.Add(idea.Id.ToString(),idea.Description,null);
          }
        }

        public IdeaController(TabControl area, IdeaService ideaService, ILoggingController loggingController) 
        {
            _area = area;
            this._ideaService = ideaService;
            _loggingController = loggingController;
            BuildUIArea(_area);
            var ideaList = GetIdeaList(_area);
            PopulateData((ListView)ideaList);

            _loggingController.Log(MessageType.information, "IdeaController Constructed");
        }

        private static Control GetIdeaList(TabControl area)
        {
            var table = area.TabPages[1].Controls[0];
            var ideaList = table.Controls[0];
            return ideaList;
        }

        public string[] GetIdeaList()
        {
            var ideas = ((ListView)GetIdeaList(_area)).Items;
            var textIdeas = TransformIdeas(ideas);
            return textIdeas.ToArray();
        }

        private List<string> TransformIdeas(ListView.ListViewItemCollection ideas)
        {
            var textIdeas = new List<string>();

            foreach (ListViewItem idea in ideas)
            {
                textIdeas.Add(idea.Text.Trim());
            }

            return textIdeas;
        }

        private void BuildUIArea(TabControl area)
        {
            var itemList = new ListView {CheckBoxes = true, Dock = DockStyle.Fill};
            //add context menu 
            itemList.DoubleClick += ItemList_DoubleClick;
            var tabPage = new TabPage {Dock = DockStyle.Fill,Text = "Ideas"};
            var tableLayout = new TableLayoutPanel {ColumnCount = 1, RowCount = 2,Dock = DockStyle.Fill};
            tableLayout.Controls.Add(itemList,0,1);
            var buttonLayout = new FlowLayoutPanel {FlowDirection = FlowDirection.LeftToRight,Height = 25,Dock = DockStyle.Top};

            var checkedAllButton = new Button() { Text = "X", Width = 30, Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold), TextAlign = ContentAlignment.TopCenter };

            checkedAllButton.Click += CheckedAllButton_Click;
           
            buttonLayout.Controls.Add(checkedAllButton);

            var addButton = new Button(){Text="+",Width = 30,Font = new Font(FontFamily.GenericSerif,10,FontStyle.Bold),TextAlign = ContentAlignment.TopCenter};
            addButton.Click += AddButton_Click;
            buttonLayout.Controls.Add(addButton);
            var deleteButton = new Button(){Text = "-", Width = 30, Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold), TextAlign = ContentAlignment.TopCenter };
            deleteButton.Click += DeleteButton_Click;
            buttonLayout.Controls.Add(deleteButton);
            tableLayout.Controls.Add(buttonLayout,0,1);
            
            tabPage.Controls.Add(tableLayout);
            area.TabPages.Add(tabPage);
            itemList.View = View.List;
            
        }

        private void CheckedAllButton_Click(object sender, System.EventArgs e)
        {
            var listView = ((ListView)GetIdeaList(_area));

            if (listView.CheckedItems.Count > 0)
            {
                foreach (ListViewItem listViewCheckedItem in listView.CheckedItems)
                {
                    listViewCheckedItem.Checked = false;
                }

                return;
            }

            foreach (ListViewItem listViewItem in listView.Items)
            {
                listViewItem.Checked = true;
            }
        }

        private void ItemList_DoubleClick(object sender, System.EventArgs e)
        {
            var listView = ((ListView)sender);
            if (listView.SelectedItems.Count == 0)
            {
                return;

            }
            var selectedItem= listView.SelectedItems[0];
            EditItem(selectedItem);
        }

        private void EditItem(ListViewItem selectedItem)
        {
            //TODO: open a edit form 
            //Show full  text 
            var itemId = selectedItem.Name;
            var itemText = selectedItem.Text;
            var editIdeaDialog = new EditIdeaDialog();
            var editedDescription= editIdeaDialog.ShowDialog(int.Parse(itemId),_ideaService);
            if (editedDescription == string.Empty)
            {
                return;
            }
            _ideaService.Update(editedDescription, itemId);
            var ideaList = (ListView)GetIdeaList(_area);
            ideaList.Items.Clear();
            PopulateData(ideaList);
        }

        private void DeleteButton_Click(object sender, System.EventArgs e)
        {
            var selectedIdeas = ((ListView)GetIdeaList(_area)).CheckedItems;
            foreach (ListViewItem idea in selectedIdeas)
            {
                idea.Remove();
                _ideaService.Delete(int.Parse(idea.Name.Trim()));
            }
        }


        private void AddButton_Click(object sender, System.EventArgs e)
        {
            var input = new AddIdeaDialog().ShowDialog();
            if (input == string.Empty)
            {
                return;
            }
            var id=_ideaService.New(new Idea(input));
            var ideaList = (ListView)GetIdeaList(_area);
            ideaList.Items.Clear();
            PopulateData(ideaList);
        }
    }
}