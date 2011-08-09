using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Diagramming.Core
{
    public class DeleteTool : AbstractTool
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="toolName"></param>
        public DeleteTool(string toolName)
            : base(toolName)
        {
        }

        protected override void OnActivateTool()
        {
            base.OnActivateTool();

            DeleteCommand cmd;

            if (Selection.SelectedItems.Count > 0)
            {
                // If any one entity in the selction can't be deleted,
                // remove it from the selection.
                for (int i = 0; i < Selection.SelectedItems.Count; i++ )
                {
                    IDiagramEntity entity = Selection.SelectedItems[i];
                    if (entity.AllowDelete == false)
                    {
                        Selection.SelectedItems.Remove(entity);
                        i--;
                    }
                }
                cmd = new DeleteCommand(
                        this.Controller,
                        Selection.SelectedItems.Copy());
                this.Controller.UndoManager.AddUndoCommand(cmd);

                // Alert each entity that they're about to be deleted.
                foreach (IDiagramEntity entity in Selection.SelectedItems)
                {
                    entity.OnBeforeDelete(cmd);
                }

                cmd.Redo();

                // Alert each entity that they have been deleted.
                foreach (IDiagramEntity entity in Selection.SelectedItems)
                {
                    entity.OnAfterDelete(cmd);
                }
            }

            DeactivateTool();
        }
    }
}
