using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.ValidationRules;

namespace InventoryManager
{
    public class UniqueIdCellValidationRule : CellValidationRule
    {
        public UniqueIdCellValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo culture, CellValidationContext context)
        {
            // Get the DataItem from the context and cast it to a DataRow
            DataRowView dataRowView = context.DataItem as DataRowView;

            // Convert the value to a long to make sure it is numerical.
            // When the value is not numerical, then an InvalidFormatException will be thrown.
            // We let it pass unhandled to demonstrate that an exception can be thrown when validating
            // and the grid will handle it nicely.
            long id = Convert.ToInt64(value, CultureInfo.CurrentCulture);

            // Try to find another row with the same ID
            System.Data.DataRow[] existingRows = dataRowView.Row.Table.Select(context.Cell.FieldName + "=" + id.ToString(CultureInfo.InvariantCulture));

            // If a row is found, we return an error
            if ((existingRows.Length != 0) && (existingRows[0] != dataRowView.Row))
                return new ValidationResult(false, "The value must be unique");

            // If no row was found, we return a ValidResult
            return ValidationResult.ValidResult;
        }
    }
}
