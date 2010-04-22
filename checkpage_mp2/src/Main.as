package 
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	import flash.text.TextField;
	import flash.ui.Keyboard;
	
	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class Main extends Sprite 
	{
		private var layouter:Layouter;
		private var debug:TextField = new TextField();
		
		public function Main():void 
		{
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
		}
		
		private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);
			// entry point
			ExternalInterface.addCallback("j2fClick", onJSEvent);
			
			layouter = new Layouter(this);
			layouter.addEventListener(ImageButtonEvent.CLICK, onClick);
			
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			
			debug.x = 480;
			debug.y = 82;
			debug.width = 300;
			debug.text = "debug";
			//addChild(debug);
			
			ExternalInterface.call("f2jClick", "initialized");
		}
		
		private function onJSEvent(msg:String):void
		{
			debug.text = "called:" + msg;
			
			var s_num:String;
			var page:int;
			
			// e.g. page_set:1,asset/hoge.swf
			if (msg.indexOf("page_set:") == 0)
			{
				s_num = msg.substring("page_set:".length);
				page = -1;
				var list:Array = s_num.split(",");

				try { page = parseInt(list[0]); }
				catch (err:Error) { }
				
				if (page != -1 && list[1] != null && list[1] != "")
				{
					layouter.setPageList(page, list[1]);
					if (page == layouter.page)
						layouter.setPage(layouter.page);
					debug.text = "page_set page:" + page + " url:" + list[1];
				}
			}
			
			else if (msg.indexOf("page") == 0)
			{
				s_num = msg.substring("page".length);
				page = -1;

				try { page = parseInt(s_num); }
				catch (err:Error) {}
				
				if (page != -1 && layouter.page != page)
					layouter.setPage(page);
			}
		}
		
		private function onClick(event:ImageButtonEvent):void
		{
			ExternalInterface.call("f2jClick", event.id);
		}
		
		private function onKeyDown(event:KeyboardEvent):void
		{
			if (event.ctrlKey && event.shiftKey && event.keyCode == Keyboard.RIGHT)
				layouter.next();
				
			else if (event.ctrlKey && event.shiftKey && event.keyCode == Keyboard.LEFT)
				layouter.prev();
		}
	}
}