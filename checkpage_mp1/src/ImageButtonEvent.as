package  
{
	import flash.events.Event;
	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class ImageButtonEvent extends Event
	{
		public static const CLICK:String = "image_button_event_click";
		
		private var _id:String;
		
		public function get id():String { return _id; }
		
		public function ImageButtonEvent(type:String, id:String) 
		{
			super(type);
			
			_id = id;
		}
		
	}

}