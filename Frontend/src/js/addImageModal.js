
$('#addImagesButton').click(function(){

	$.Dialog({
		overlay: true,
		shadow: true,
		flat: true,
		draggable: true,
		icon: '<i class="icon-image on-left"></i>',
		title: 'Agregar imagenes',
		onShow: function(_dialog){

			var content = _dialog.children('.content');
			var addImageForm = $('.add-image-modal').clone();
			content.html(addImageForm.show());

			var internetInput = $($('.add-internet-input')[1]);
			var pcInput = $($('.add-pc-input')[1]);
			var imagePreview = $($('.add-image-preview')[1]);

			var addFromPcSpan = $($('.add-pc')[1]).siblings()[0];
			$(addFromPcSpan).click(function(){
				var isAddFromPcChecked = $($('.add-pc')[1]).not(':checked');

				if (isAddFromPcChecked) {
					internetInput.attr('disabled', 'disabled');
					pcInput.removeAttr('disabled');
					internetInput.val('');
					imagePreview.attr('src', '../assets/images/image-placeholder.png');
				}
			});

			var addFromInternetSpan = $($('.add-internet')[1]).siblings()[0];
			$(addFromInternetSpan).click(function(){
				var isAddFromInternetChecked = $($('.add-internet')[1]).not(':checked');

				if (isAddFromInternetChecked) {
					pcInput.attr('disabled', 'disabled');
					internetInput.removeAttr('disabled');
					pcInput.val('');
					pcInput.siblings().val('');
					imagePreview.attr('src', '../assets/images/image-placeholder.png');
				}
			});


			// Image from internet: show preview
			$($('.add-internet-input')[1]).on('blur', function(){
				if (internetInput.val() != '') {
					$($('.add-image-preview')[1]).attr('src', $($('.add-internet-input')[1]).val());
				}
			});

			// Submit button
			$($('.add-accept')[1]).click(function(){
				var userUploaded = $($('.add-pc')[1]).is(':checked');

				var imagen = {
					name : $($('.add-name-input')[1]).val(),
					userUploaded : userUploaded,
					path : ''
				};

				if (!userUploaded) {
					imagen.originalURL = internetInput.val();
				} else {
					console.log("as", pcInput.val());
				}

				$.post(postImage, imagen, function( data ) {
					addImageToBoard(data);
					$.Dialog.close();
				}).fail(function(obj, status, message){
					// TODO: Handle error
				});
			});

			// Cancel button
			$($('.add-cancel')[1]).click(function(){
				$.Dialog.close();
			});

			// Input file change
			pcInput.on('change', function() {
				pcInput.siblings().val(pcInput.val());
			});
		}
	});
});
