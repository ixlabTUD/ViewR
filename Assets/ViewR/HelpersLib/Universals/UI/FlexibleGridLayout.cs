using UnityEngine;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.Universals.UI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
    
        [Header("Public exposed settings")]
        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;
    
        [Header("Debugging")]
        [SerializeField, ReadOnly] private bool fitX;
        [SerializeField, ReadOnly] private bool fitY ;
        public bool debugging ;
    
    
    
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
        
            if(debugging) Debug.Log($"Re-Calculating Layout".StartWithFrom(GetType()), this);

            fitX = fitType == FitType.Width || fitType == FitType.Uniform;
            fitY = fitType == FitType.Height || fitType == FitType.Uniform;
        
            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                var sqrt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrt);
                columns = Mathf.CeilToInt(sqrt);
            }

            if(fitType == FitType.Width || fitType == FitType.FixedColumns)
                // we'll set columns in inspector
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
                // we'll set rows in inspector
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);

            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            var cellWidth = parentWidth / columns - spacing.x / columns * (columns - 1) - padding.left / (float) columns - padding.right / (float) columns;
            var cellHeight = parentHeight / rows - spacing.y / rows * (rows - 1 ) - padding.bottom / (float) rows - padding.top / (float) rows;
        

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y ;


            for (var i = 0; i < rectChildren.Count; i++)
            {
                var currentRowIndex = i / columns;
                var currentColumnIndex = i % columns;

                var item = rectChildren[i];

                var xPos = cellSize.x * currentColumnIndex + spacing.x * currentColumnIndex + padding.left;
                var yPos = cellSize.y * currentRowIndex + spacing.y * currentRowIndex + padding.top;
            
                SetChildAlongAxis(item, 0 ,xPos,cellSize.x);
                SetChildAlongAxis(item, 1 ,yPos,cellSize.y);
            
            }
        }
    
    
        public override void SetLayoutHorizontal()
        {
            if(debugging) Debug.Log($"SetLayoutHorizontal".StartWithFrom(GetType()), this);
        }
    
        public override void CalculateLayoutInputVertical()
        {
            if(debugging) Debug.Log($"CalculateLayoutInputVertical".StartWithFrom(GetType()), this );
        }


        public override void SetLayoutVertical()
        {
            if(debugging) Debug.Log($"SetLayoutVertical".StartWithFrom(GetType()), this);
        }
    }
}
