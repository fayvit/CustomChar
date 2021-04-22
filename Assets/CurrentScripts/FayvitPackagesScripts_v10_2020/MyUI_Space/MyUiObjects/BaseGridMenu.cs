using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI_10_2020
{
    public abstract class BaseGridMenu : InteractiveUiBase
    {
        protected System.Action<int> ThisAction;
        protected int lineCellCount = 0;
        protected int rowCellCount = 0;

        public void ChangeOption(int Vval, int Hval)
        {

            int quanto = -lineCellCount * Vval;

            if (quanto == 0)
                quanto = Hval;

            ChangeOptionWithVal(quanto, lineCellCount);

        }

        protected int LineCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            Debug.Log("grid lengths: " + grid.cellSize + " : " + grid.spacing.x);

            return
                (int)((variableSizeContainer.rect.width - grid.padding.left - grid.padding.right) / (grid.cellSize.x + grid.spacing.x));
        }

        protected int RowCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            return
                (int)(variableSizeContainer.rect.height / (grid.cellSize.y + grid.spacing.y));
        }

        public void SetLineRowLength()
        {
            lineCellCount = LineCellCount();
            rowCellCount = RowCellCount();

            Debug.Log("Celulas do grid: " + lineCellCount + " : " + rowCellCount);
        }

        protected override void AfterFinisher()
        {
            ThisAction = null;
        }
    }
}
