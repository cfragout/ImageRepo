$(function(){

	$('.image-container').click(function(){
		var isSelected = $(this).hasClass('selected');

		$('.image-container').removeClass('selected');
		if (isSelected) {
			$(this).removeClass('selected');
			$('.actions span').addClass('fg-gray no-hover');
			$('.actions').addClass('no-hover');
		} else {
			$('.actions span').removeClass('fg-gray no-hover');
			$('.actions').removeClass('no-hover');
			$(this).addClass('selected');
		}

	});


});