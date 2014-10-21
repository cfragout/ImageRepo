function readURL(inputHTML, imageHTML) {
	if (inputHTML.files && inputHTML.files[0]) {
		var reader = new FileReader();

		reader.onload = function (e) {
			$(imageHTML).attr('src', e.target.result);
		}

		reader.readAsDataURL(inputHTML.files[0]);
	}
}