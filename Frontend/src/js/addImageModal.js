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
			var tagsInput = $($('.add-tags-input')[1]);
			var nameInput = $($('.add-name-input')[1]);


			// Disable upload from pc input
			pcInput.siblings().attr('disabled', 'disabled');
			pcInput.attr('disabled', 'disabled');

			// init tags plugin
			tagsInput.tagit({
				fieldName: "Tags",
				caseSensitive: false,
				removeConfirmation: true,
				autocomplete: { delay: 0, minLength: 2 },
				availableTags: ['gifs', 'gracioso', 'NSFW', 'animadas', 'internet', 'wallpaper', 'autos', 'autos1', 'autos2', 'fotos', 'trabajo'], // Get these from the server
				placeholderText: "gifs, gracioso, NSFW"
			});

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
			pcInput.on('change', function(){
				var file = this.files[0];
				var name = file.name;
				var size = file.size;
				var type = file.type;
				filename = name;

				readURL(this, $('.add-image-preview')[1]);
			});


			// Image from internet: show preview
			internetInput.on('change', function(){
				if (internetInput.val() != '') {
					$($('.add-image-preview')[1]).attr('src', internetInput.val());
				}
			});

			// Submit button
			$($('.add-accept')[1]).click(function() {

				internetInput.parent().removeClass('validation-error');
				pcInput.parent().removeClass('validation-error');
				nameInput.parent().removeClass('validation-error');


				if (!validImage()) {
					return;
				}

				var userUploaded = $($('.add-pc')[1]).is(':checked');

				$($('.add-image-error-section')[1]).hide();

				showLoadingScreen(true);

				var imagen = {
					name : $($('.add-name-input')[1]).val(),
					userUploaded : userUploaded,
					path : '',
					tags : getArrayOfTagObjects()
				};

				if (!userUploaded) {
					imagen.originalURL = internetInput.val();
					postImageObj(imagen);
				} else {
					var formData = new FormData($('.add-image-form')[1]);

					formData.append('url', filename);

					$.ajax({
						type: "POST",
						url: uploadImageUrl,
						data: formData,
						cache: false,
						contentType: false,
						processData: false,
						success: function(data) {
							addImageToBoard(data);
							showLoadingScreen(false);
							$.Dialog.close();
						},
						error: function() {
							showLoadingScreen(false);
							$($('.add-image-error')[1]).text('Hubo un problema al cargar la imagen. Intentalo nuevamente.');
							$($('.add-image-error-section')[1]).show();
						}
					});

				}

			});

			// Cancel button
			$($('.add-cancel')[1]).click(function(){
				$.Dialog.close();
			});

			// Input file change
			pcInput.on('change', function() {
				pcInput.siblings().val(pcInput.val());
			});

			// Clear input button
			internetInput.siblings().click(function(){
				internetInput.val('');
				imagePreview.attr('src', '../assets/images/image-placeholder.png');
			});
		}
	});
});

function showLoadingScreen(show) {
	if (show) {
		$($('.add-image-loading-screen')[1]).fadeIn();
	} else {
		$($('.add-image-loading-screen')[1]).fadeOut();
	}
}

function validImage() {
	var internetInput = $($('.add-internet-input')[1]);
	var pcInput = $($('.add-pc-input')[1]);
	var nameInput = $($('.add-name-input')[1]);
	var pcCheck = $($('.add-pc')[1]);
	var internetCheck = $($('.add-internet')[1]);
	var isValid = true;

	if (nameInput.val() == '') {
		isValid = false;
		nameInput.parent().addClass('validation-error');
	}

	if ((internetInput.val() == '') && (internetCheck.is(':checked'))) {
		isValid = false;
		internetInput.parent().addClass('validation-error');
	}

	if ((pcInput.val() == '') && (pcCheck.is(':checked'))) {
		isValid = false;
		pcInput.parent().addClass('validation-error');
	}

	return isValid;
}

// Sends image data (such as name, originalURL, etc) to the server
function postImageObj(imageObj) {
	$.post(postImageUrl, imageObj, function( data ) {
		addImageToBoard(data);
		showLoadingScreen(false);
		$.Dialog.close();

	}).fail(function(obj, status, message){
		showLoadingScreen(false);
		console.log(message);
		$($('.add-image-error')[1]).text('Hubo un problema al cargar la imagen. Intenta nuevamente.');
		$($('.add-image-error-section')[1]).show();
	});
}

// Gets the value of the hidden input that saves user entered tags and creates an array of tag objects.
function getArrayOfTagObjects() {
	var tags = $($('.add-tags-input')[1]).val();

	if ((tags == '') || (tags == null)) {
		return [];
	}

	var tagsArray = tags.split(',');
	var tagObjectsArray = [];
	for (var tag in tagsArray) {
		tagObjectsArray.push({
			name: tagsArray[tag]
		});
	}

	return tagObjectsArray;
}