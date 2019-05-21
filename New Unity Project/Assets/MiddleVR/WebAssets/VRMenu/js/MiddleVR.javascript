/*! MiddleVR.js | (c) 2014 i'm in VR. | www.imin-vr.com */

/*
 * String helper
 */

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
  String.prototype.format = function() {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function(match, number) { 
      return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
    });
  };
}

/*
 * MiddleVR.Call for debugging in Chrome
 */

if (typeof MiddleVR == "undefined") {
	var MiddleVR = {};
	(function() {
		MiddleVR.Call = function(name, value) {
			console.log("MiddleVR.Call({0},{1})".format(JSON.stringify(name),JSON.stringify(value)));
		};
	})();
}

/*
 * Global widget container
 */
var vrWidgets = {};

/*
 * Base class
 */

/* WARNING : everything in options is passed by VALUE */

var vrWidget = new Class({
	Implements: Options,
	options: {
		name: "",
		command: "",
		label: ""
	},
	initialize: function( options, parent ) {
		this.setOptions( options );

		this.parent = (typeof parent !== 'undefined' ? parent : "#body");
		this.children = new Array();

		this.container = null;
		this.element = null;

		this.Create();

		if ( typeOf(this.parent) == "string" )
		{
			this.element.appendTo( this.parent );
		}
		else if ( typeOf(this.parent) == "object" )
		{
			this.parent.AddChild( this );
		}
	},
	Create: function() {
		this.element = $( "<div></div>" );
	},
	RefreshWidget: function() {
	},
	AddChild: function( widget ) {
		this.children.push( widget );
		widget.element.appendTo( this.element );
	},
	Destroy: function() {
		if ( typeOf(this.parent) == "object" )
		{
			var i = this.parent.children.indexOf( this );
			if( i != -1 ) {
				this.parent.children.splice( i, 1 );
			}
		}

		if( this.container ) {
			this.container.remove();
		}
		else {
			this.element.remove();
		}
	},
	SetLabel: function( label ) {
		this.options.label = label;
		if( this.label )
		{
			this.label.text( this.options.label );
		}
	},
	MoveChild: function( oldIndex, newIndex ) {
		var child = this.element.children().eq( oldIndex );
		child.detach();
		var lastIndex = this.element.children().size();
		child.appendTo( this.element );
		if (newIndex < lastIndex) {
			this.element.children().eq( newIndex ).before( this.element.children().last() );
		}
	},
	RemoveChild: function( widget ) {
		widget.element.remove();
		var indexOfWidget = this.children.indexOf( widget );
		if (indexOfWidget > -1) {
		    this.children.splice(indexOfWidget, 1);
		}
	},
	SetParent: function( newParent ) {
		if ( typeOf(this.parent) == "object" )
		{
			this.parent.RemoveChild( this );
		}
		else
		{
			$( this.parent ).remove( this.element );
		}

		this.parent = (typeof newParent !== 'undefined' ? newParent : "#body");

		if ( typeOf(this.parent) == "string" )
		{
			this.element.appendTo( this.parent );
			this.RefreshWidget();
		}
		else if ( typeOf(this.parent) == "object" )
		{
			this.parent.AddChild( this );
			this.RefreshWidget();
		}
	}
});

var vrWidgetGroup = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultGroupName",
		label:  "DefaultGroupLabel",
	},
	Create: function() {
		this.element = $( '<div class="panel panel-default"></div>' );
		this.label = $( '<div class="panel-heading">{0}</div>'.format(this.options.label) ).appendTo( this.element );
		this.panel_body = $( '<div class="panel-body"></div>' ).appendTo( this.element );
	},
	AddChild: function( widget ) {
		this.children.push( widget );
		widget.element.appendTo( this.panel_body );
	}
});

var vrWidgetMenu = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultMenuName",
		label:  "DefaultMenuLabel",
		visible: false
	},
	Create: function() {
		if ( typeOf(this.parent) == "object" && instanceOf(this.parent, vrWidgetMenu) )
		{
			this.element = $( '<ul class="dropdown-menu"></ul>' );
		}
		else
		{
			this.element = $( '<ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu" style="display: block; position: static; margin-bottom: 5px; *width: 180px;">' );
		}
	},
	Callback: function( event ) {
		this.CloseSiblings();
		this.SetVisibleNotify( !this.container.hasClass('open') );
	},
	AddChild: function( widget ) {
		this.children.push( widget );
		if( instanceOf(widget, vrWidgetMenu) )
		{
			widget.container = $('<li class="dropdown-submenu"></li>');
			widget.label = $('<a href="#">{0}</a>'.format(widget.options.label)).appendTo(widget.container);
			widget.element.appendTo(widget.container);
			widget.container.appendTo( this.element );

			widget.SetVisible( widget.options.visible );

			widget.label.on('click', $.proxy( widget.Callback, widget ) );
		}
		else if( instanceOf(widget, vrWidgetButton) )
		{
			widget.container = $( '<li></li>' ).appendTo(this.element);
			widget.element.appendTo( widget.container );

			// Don't let the menu event handler handle click event inside this container
			widget.container.click(function (e) {
		      e.stopPropagation();
		    });
		}
		else if( instanceOf(widget, vrWidgetSeparator) )
		{
			widget.container = $( '<li role="presentation" class="divider"></li>' ).appendTo(this.element);
		}
		else
		{
			widget.container = $( '<li></li>' ).appendTo(this.element);
			widget.container_a = $( '<a></a>').appendTo(widget.container);
			widget.element.appendTo( widget.container_a );
		}
	},
	RefreshWidget: function()
	{
		for (var i = 0; i < this.children.length; i++)
		{
		    var widget = this.children[i];
		    widget.RefreshWidget();

		    if( instanceOf(widget, vrWidgetMenu) )
		    {
		    	widget.label.on('click', $.proxy( widget.Callback, widget ) );
		    }
		    else if( instanceOf(widget, vrWidgetButton) )
		    {
		    	widget.container.click(function (e) {
			      e.stopPropagation();
			    });
		    }
		}
	},
	RemoveChild: function( widget ) {
		widget.container.remove();
		widget.container = null;

		var indexOfWidget = this.children.indexOf( widget );
		if (indexOfWidget > -1) {
		    this.children.splice(indexOfWidget, 1);
		}
	},
	SetVisible: function( isVisible ) {
		if( this.container )
		{
			if( isVisible )
			{
				this.CloseSiblings();
				this.container.addClass('open');
			}
			else
			{
				this.container.removeClass('open');
			}
		}
		this.options.visible = isVisible;
	},
	CloseSiblings: function() {
		if ( typeOf(this.parent) == "object" && instanceOf(this.parent, vrWidgetMenu) )
		{
			for( var i = 0; i < this.parent.children.length; ++i )
			{
				if( instanceOf(this.parent.children[i], vrWidgetMenu) && this.parent.children[i] !== this )
				{
					this.parent.children[i].SetVisibleNotify( false );
				}
			}
		}
	},
	SetVisibleNotify: function( isVisible ) {
		if( this.container ) {
			if( isVisible && !this.container.hasClass('open') ) {
				this.container.addClass('open');
				MiddleVR.Call(this.options.command, true);
			}
			else if( !isVisible && this.container.hasClass('open') ) {
				this.container.removeClass('open');
				MiddleVR.Call(this.options.command, false);
			}
		}
		this.options.visible = isVisible;
	}
});

var vrWidgetButton = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultButtonName",
		label:   "DefaultButtonLabel",
	},
	Callback: function() {
		MiddleVR.Call(this.options.command, null);
	},
	Create: function() {
		if ( typeOf(this.parent) == "object" && instanceOf(this.parent, vrWidgetMenu) )
		{
			this.element = $( '<a href="#">{0}</a>'.format(this.options.label) );
		}
		else
		{
			this.element = $( '<button>{0}</button>'.format(this.options.label) );
		}
		this.RefreshWidget();
	},
	RefreshWidget: function() {
		this.element.on("click", $.proxy(this.Callback,this));
	},
	SetLabel: function( label ) {
		this.options.label = label;
		if( this.element )
		{
			this.element.text( this.options.label );
		}
	}
});

var vrWidgetSeparator = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultSeparatorName",
		label:   "",
	},
	Create: function() {
		if ( typeOf(this.parent) == "object" && instanceOf(this.parent, vrWidgetMenu) )
		{
			this.element = null;
		}
		else
		{
			this.element = $( '<hr>' );
		}
	},
	SetLabel: function( label ) {
		this.options.label = label;
	}
});

var vrWidgetToggleButton = new Class({
	Extends: vrWidget,
	options: {
		name: "DefaultToggleButtonName",
		label: "DefaultToggleButtonLabel",
		isChecked: false
	},
	Callback: function() {
		MiddleVR.Call(this.options.command, this.input.is(':checked') );
	},
	Create: function() {
		this.element = $( "<label></label>" );
		
		this.input = $( '<input  class="ui-button" type="checkbox" />' ).appendTo( this.element );
		this.input.prop( "checked", this.options.isChecked );

		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.RefreshWidget();
	},
	RefreshWidget: function() {
		this.input.on("change", $.proxy(this.Callback,this));
	},
	SetChecked: function( checked ) {
		this.options.isChecked = checked;
		this.input.prop("checked",this.options.isChecked);
	}
});

var vrWidgetRadioButton = new Class({
	Extends: vrWidget,
	options: {
		name: "DefaultRadioButtonName",
		groupName: "DefaultRadioButtonGroupName",
		label: "DefaultRadioButtonLabel",
		isChecked: false
	},
	Callback: function() {
		MiddleVR.Call(this.options.command, true);
	},
	Create: function() {
		this.element = $( "<label></label>" );
		
		this.input = $( '<input type="radio" name="{0}" id="{1}" />'.format(this.options.groupName,this.options.name) ).appendTo( this.element );
		this.input.prop( "checked", this.options.isChecked);

		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.RefreshWidget();
	},
	RefreshWidget: function() {
		this.input.on("click", $.proxy(this.Callback,this));
	},
	SetChecked: function( checked ) {
		this.options.isChecked = checked;
		this.input.prop( "checked", this.options.isChecked);
	}
});

var vrWidgetSlider = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultSliderName",
		label:  "DefaultSliderLabel",
		value: 1,
		step:  1,
		min:   0,
		max:  10
	},
	Callback: function( e ) {
		this.options.value = Number( this.input.slider('getValue') );
		MiddleVR.Call(this.options.command, this.options.value);
	},
	Create: function() {
		this.element = $( "<label></label>" );

		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.input = null;
		this.RefreshWidget();
	},
	RefreshWidget: function() {
		if(this.input != null)
		{
			this.element.empty();
			this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );
		}
		this.input = $( '<input type="text" />' ).appendTo( this.element ).slider( { min: this.options.min, max: this.options.max, step: this.options.step, value: this.options.value });
		this.input.on("slideStop", $.proxy(this.Callback,this));
	},
	SetMin: function( min_ ) {
		this.options.min = min_;
		this.input.slider('setAttribute', "min", this.options.min );
	},
	SetMax: function( max_ ) {
		this.options.max = max_;
		this.input.slider('setAttribute', "max", this.options.max );
	},
	SetStep: function( step_ ) {
		this.options.step = step_;
		this.input.slider('setAttribute', "step", this.options.step );
	},
	SetValue: function( value_ ) {
		this.options.value = value_;
		this.input.slider('setValue', this.options.value );
	}
});

var vrWidgetColorPicker = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultColorPickerName",
		label:  "DefaultColorPickerLabel",
		color: {r:0,g:0,b:0}
	},
	Callback: function(hsb,hex,rgb,el,bySetColor) {
		if( ! bySetColor ) {
			this.options.color = rgb;
			MiddleVR.Call( this.options.command, rgb );
		}
	},
	Create: function() {
		this.element = $( '<label></label>' );
		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.input = null;
		this.RefreshWidget();
	},
	RefreshWidget: function() {
		if( this.input != null )
		{
			this.input.remove();
		}
		this.input = $( '<div></div>' ).appendTo( this.element );
		this.input.colpick({color:this.options.color, flat:true, layout:'hex', submit:0, onChange: $.proxy(this.Callback, this) });
	},
	SetColor: function( rgb ) {
		this.options.color = rgb;
		this.input.colpickSetColor( this.options.color, true );
	}
});

var vrWidgetList = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultListName",
		label:  "DefaultListLabel",
		list:   "",
		selectedIndex: -1
	},
	Callback: function() {
		this.options.selectedIndex = this.input[0].selectedIndex;
		MiddleVR.Call(this.options.command,this.options.selectedIndex);
	},
	Create: function() {
		this.element = $( '<label></label>' );

		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.input = null;
		this.RefreshWidget();
	},
	RefreshWidget: function() {
		if(this.input != null)
		{
			this.element.empty();
			this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );
		}
		this.input = $( '<select></select>' ).appendTo( this.element );
		this.SetList( this.options.list );
		this.SetSelectedIndex( this.options.selectedIndex );
		this.input.selectpicker();
		this.input.on("change", $.proxy(this.Callback,this));
	},
	SetSelectedIndex: function( selectedIndex ) {
		this.options.selectedIndex = selectedIndex;
		this.input[0].selectedIndex = this.options.selectedIndex;
		this.input.selectpicker('refresh');
	},
	SetList: function( list ) {
		this.input.empty();
		this.options.list = list;
		for (i = 0; i < this.options.list.length; ++i) {
			if( i == this.options.selectedIndex ) {
				$( '<option selected="true">{0}</option>'.format(this.options.list[i]) ).appendTo(this.input);
			}
			else {
				$( '<option>{0}</option>'.format(this.options.list[i]) ).appendTo(this.input);
			}
		}
		this.input.selectpicker('refresh');
	}
});

var vrWidgetTextInput = new Class({
	Extends: vrWidget,
	options: {
		name:   "DefaultTextInputName",
		label:  "DefaultTextInputLabel",
		text:   ""
	},
	Callback: function() {
		this.options.text = this.input.val();
		MiddleVR.Call(this.options.command, this.options.text);
	},
	Create: function() {
		this.element = $( '<label></label>' );

		this.label = $( '<span>{0}</span>'.format(this.options.label) ).appendTo( this.element );

		this.input = $( '<input type="text" />' ).appendTo( this.element );
		this.input.prop( "value", this.options.text );
		
		this.RefreshWidget();
	},
	RefreshWidget: function() {
		this.input.on("change", $.proxy(this.Callback,this));
	},
	SetText: function( text ) {
		this.options.text = text;
		this.input.val( text );
	}
});