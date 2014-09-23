var filename = "";

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

			// Disable upload from pc input
			pcInput.siblings().attr('disabled', 'disabled');
			pcInput.attr('disabled', 'disabled');

			var addFromPcSpan = $($('.add-pc')[1]).parent()[0];
			$(addFromPcSpan).click(function(){
				var isAddFromPcChecked = $($('.add-pc')[1]).not(':checked');

				if (isAddFromPcChecked) {
					internetInput.attr('disabled', 'disabled');
					pcInput.siblings().removeAttr('disabled');
					pcInput.removeAttr('disabled');
					internetInput.val('');
					imagePreview.attr('src', '../assets/images/image-placeholder.png');
				}
			});

			var addFromInternetSpan = $($('.add-internet')[1]).parent()[0];
			$(addFromInternetSpan).click(function(){
				var isAddFromInternetChecked = $($('.add-internet')[1]).not(':checked');

				if (isAddFromInternetChecked) {
					pcInput.siblings().attr('disabled', 'disabled');
					pcInput.attr('disabled', 'disabled');
					internetInput.removeAttr('disabled');
					pcInput.val('');
					pcInput.siblings().val('');
					imagePreview.attr('src', '../assets/images/image-placeholder.png');
				}
			});

			// File input on change
			$($(':file')[1]).on('change', function(){
				var file = this.files[0];
				var name = file.name;
				var size = file.size;
				var type = file.type;
				console.log("file",file);
				filename = name;

				readURL(this);
			});


			// Image from internet: show preview
			internetInput.on('change', function(){
				if (internetInput.val() != '') {
					$($('.add-image-preview')[1]).attr('src', internetInput.val());
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
					imagen.originalURL = filename;

					var formData = new FormData($('.add-image-form')[1]);
					$.ajax({
						type: "POST",
						url: baseURL + '/Home/UploadPcImage',
						data: formData,
						cache: false,
						contentType: false,
						enctype: 'multipart/form-data',
						processData: false
					});

				}
console.log("imagen", imagen);
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

// Image from pc: show preview
function readURL(input) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function (e) {
			$($('.add-image-preview')[1]).attr('src', e.target.result);
		}

		reader.readAsDataURL(input.files[0]);
	}
}