package  
{
	import flash.display.DisplayObjectContainer;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.MouseEvent;
	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class Layouter extends EventDispatcher
	{
		private const min:uint = 1;
		private const max:uint = 6;
		
		private var _main:Sprite;
		private var _page:int = 0;
		private var _containa:Sprite;
		private var _pagelist:Array;
		
		public function get page():int { return _page; }
		
		public function Layouter(main:Sprite) 
		{
			_main = main;
			
			_pagelist = new Array(
				"assets/v01_01.swf",
				"assets/v01_02.swf",
				"assets/v01_03.swf",
				"assets/v01_04.swf",
				"assets/v01_05.swf",
				"assets/v01_06.swf"
				);

			next();
		}
		
		public function clean():void
		{
			if (_containa != null)
			{
				_main.removeChild(_containa);
				_containa = null;
			}
		}
		
		public function prev():void
		{
			if (--_page < min)
				_page = min;
				
			setPage(_page);
		}
		
		public function next():void
		{
			if (++_page > max)
				_page = max;
				
			setPage(_page);
		}
		
		public function setPage(index:int):void
		{
			if (index < min || max < index)
				return;

			_page = index;
			
			clean();
			
			_containa = new Sprite();
			_main.addChild(_containa);
			
			loadBG(_pagelist[index - 1]);
			switch (index)
			{
				case 1:
					loadImage(1.4, 82.2, "assets/images/icon_question.png");
					loadButton("faq01", 20.4, 84.2, "assets/images/faqlink01.png", "assets/images/faqlink01_on.png");
					break;
				case 2:
					loadButton("next", 640, 26, "assets/images/button_next.png", "assets/images/button_next_on.png");
					loadImage(1.4, 82.2, "assets/images/icon_question.png");
					loadButton("faq02", 20.4, 84.2, "assets/images/faqlink02.png", "assets/images/faqlink02_on.png");
					loadImage(230.2, 82.2, "assets/images/icon_question.png");
					loadButton("faq03", 249.4, 84.2, "assets/images/faqlink03.png", "assets/images/faqlink03_on.png");
					break;
				case 3:
					loadImage(1.4, 82.2, "assets/images/icon_question.png");
					loadButton("faq03", 20.4, 84.2, "assets/images/faqlink03.png", "assets/images/faqlink03_on.png");
					break;
				case 4:
					loadButton("yes", 640, 16, "assets/images/button_yes.png", "assets/images/button_yes_on.png");
					loadButton("no", 640, 56, "assets/images/button_no.png", "assets/images/button_no_on.png");
					break;
				case 5:
					loadButton("end_fault", 640, 26, "assets/images/button_end.png", "assets/images/button_end_on.png");
					break;
				case 6:
					loadButton("end", 640, 26, "assets/images/button_end.png", "assets/images/button_end_on.png");
					break;
			}
		}
		
		public function setPageList(index:int, url:String):void
		{
			if (index < min || max < index)
				return;
				
			_pagelist[index - 1] = url;
		}
		
		private function loadBG(path:String):void
		{
			var loader:BGLoader = new BGLoader();
			loader.loadBG(path);
			_containa.addChild(loader);
		}
		
		private function loadImage(x:Number, y:Number, path:String):void
		{
			var loader:Image = new Image();
			loader.loadImage(path);
			loader.x = x;
			loader.y = y;
			_containa.addChild(loader);
		}
		
		private function loadButton(id:String, x:Number, y:Number, upPath:String, downPath:String = null):void
		{
			var up:String = upPath;
			var down:String = (downPath == null) ? upPath : downPath;
			
			var button:ImageButton = new ImageButton(id);
			button.addEventListener(MouseEvent.CLICK, onClick);
			button.loadImage(up, down);
			button.x = x;
			button.y = y;
			_containa.addChild(button);
		}
		
		private function onClick(event:MouseEvent):void
		{
			if (event.target is ImageButton)
			{
				var button:ImageButton = event.target as ImageButton;
				dispatchEvent(new ImageButtonEvent(ImageButtonEvent.CLICK, button.id));
			}
		}
	}
}