namespace вход_курсач.Design
{
    // Сортировка списка по клику на заголовок столбца таблицы.
    // Повторный клик по тому же столбцу меняет направление сортировки.
    internal class ColumnSorter
    {
        private string _lastColumn = "";
        private bool _ascending = true;

        // propertyName — имя свойства элемента списка, по которому нужно отсортировать
        public List<T> Sort<T>(List<T> list, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
                return list;

            _ascending = _lastColumn != propertyName || !_ascending;
            _lastColumn = propertyName;

            return _ascending
                ? list.OrderBy(x => property.GetValue(x)).ToList()
                : list.OrderByDescending(x => property.GetValue(x)).ToList();
        }
    }
}
