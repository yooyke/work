package  
{
	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class Queue
	{
		private var data:Array = [];
		
		public function get length():uint { return data.length; }

		public function Queue()
		{
			data = new Array();
		}

		public function enqueue(obj:*):void
		{
			data[data.length] = obj;
		}

		public function dequeue():*
		{
			return data.splice(0, 1);
		}

		public function peek():*
		{
			return data[0];
		}

		public function empty():Boolean
		{
			return data.length == 0;
		}

		public function print():void
		{
			trace(data);
		}
	}
}