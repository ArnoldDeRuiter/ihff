$(document).ready(function() {
    $( "#firstfilter" ).trigger( "click" );
});

(function ($) {
    var $container = $('#left-div-container'),
		colWidth = function () {
			var w = $container.width(), 
				columnNum = 1,
				columnWidth = 0;
			if (w > 1200) {
				columnNum  = 5;
			} else if (w > 900) {
				columnNum  = 4;
			} else if (w > 600) {
				columnNum  = 3;
			} else if (w > 300) {
				columnNum  = 2;
			}
			columnWidth = Math.floor(w / 2.2); //ipv 2.2 (goede size test) doen: columnNum en hierboven if else columnNum vervangen aan de hand van 2.2
			$container.find('.item').each(function() {
				var $item = $(this),
					multiplier_w = $item.attr('class').match(/item-w(\d)/),
					multiplier_h = $item.attr('class').match(/item-h(\d)/),
					width = multiplier_w ? columnWidth*multiplier_w[1]-2 : columnWidth-2,
					height = multiplier_h ? columnWidth*multiplier_h[1]*0.6-2 : columnWidth*0.6-2;
				$item.css({
					width: width,
					height: height
				});
			});
			return columnWidth-220;
		},
		isotope = function () {
			$container.isotope({
				itemSelector: '.item',
				masonry: {
					columnWidth: colWidth()
				}
			});
		};
    isotope();
    $(window).on('debouncedresize', isotope);
}(jQuery));





/*FILTER*/
if ($('#left-div-container').length > 0) {
    var $container = $('#left-div-container');
                
	
	
		
		// filter items when filter link is clicked
    var $optionSets = $('.filmFilter .option-set'),
			$optionLinks = $optionSets.find('a');
	
		  $optionLinks.click(function(){
			var $this = $(this);
			// don't proceed if already selected
			if ( $this.hasClass('selected') ) {
			  return false;
			}
			var $optionSet = $this.parents('.option-set');
			$optionSet.find('.selected').removeClass('selected');
			$this.addClass('selected');
	  
			// make option object dynamically, i.e. { filter: '.my-filter-class' }
			var options = {},
				key = $optionSet.attr('data-option-key'),
				value = $this.attr('data-option-value');
			// parse 'false' as false boolean
			value = value === 'false' ? false : value;
			options[ key ] = value;
			if ( key === 'layoutMode' && typeof changeLayoutMode === 'function' ) {
			  // changes in layout modes need extra logic
			  changeLayoutMode( $this, options )
			} else {
			  // otherwise, apply new options
			  $container.isotope( options );
			}
			
			return false;
		});
	}
        