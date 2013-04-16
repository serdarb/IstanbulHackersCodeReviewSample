namespace AgileWall.Domain.Conract.RequestDto
{
    public class ItemRequestDto
    {
        private string _itemId;
        private string _text;

        public string ItemId
        {
            get { return _itemId; }
            set
            {
                if (value != null)
                {
                    _itemId = value.Trim();
                }
                _itemId = value;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value != null)
                {
                    _text = value.Trim();
                }
                _text = value;
            }
        }
    }
}