//namespace DataMigrator
//{
//    public class ExtendedTabControl : TabControl
//    {
//        public delegate void TabHeaderRightClickedHandler(TabHeaderRightClickedEventArgs e);

//        public event TabHeaderRightClickedHandler TabHeaderRightClicked;

//        protected override void OnMouseUp(MouseEventArgs e)
//        {
//            if (TabHeaderRightClicked != null)
//            {
//                if (e.Button == MouseButtons.Right)
//                {
//                    for (int i = 0; i < TabPages.Count; i++)
//                    {
//                        if (this.GetTabRect(i).Contains(e.Location))
//                        {
//                            var args = new TabHeaderRightClickedEventArgs { SelectedIndex = i };
//                            TabHeaderRightClicked(args);
//                            break;
//                        }
//                    }
//                }
//            }

//            base.OnMouseUp(e);
//        }
//    }

//    public class TabHeaderRightClickedEventArgs
//    {
//        public int SelectedIndex { get; set; }
//    }
//}