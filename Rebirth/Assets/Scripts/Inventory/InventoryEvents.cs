public class InventoryEvents
{
    public delegate void ItemSwapEvent(GridCell originalCell, GridCell targetCell);
    public static event ItemSwapEvent OnItemSwapped;

    public static void ItemSwapped(GridCell originalCell, GridCell targetCell)
    {
        OnItemSwapped?.Invoke(originalCell, targetCell);
    }
}
