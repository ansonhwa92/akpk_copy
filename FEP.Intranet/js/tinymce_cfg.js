var myUrl = document.getElementById("tmscript").getAttribute("baseUrl");
var editorUploadUrl = myUrl + '/elearning/File/EditorUpload';
var basePath = myUrl + '/eLearning';

console.log(basePath);

tinymce.init({
    content_css: ['/css/elearn.css'],
    font_formats: "Arial=arial,helvetica,sans-serif;" +
        "Arial Black=arial black,avant garde;" +
        "Book Antiqua=book antiqua,palatino;" +
        "Comic Sans MS=comic sans ms,sans-serif;" +
        "Courier New=courier new,courier;" +
        "Georgia=georgia,palatino;" +
        "Helvetica=helvetica;" +
        "Ryal Black Block=ryal black block;" +
        "Pompadour=pompadour;" +
        "Adam Gorry;" +
        "Admiration Pains;" +
        "Bad Stories;" +
        "Badang;" +
        "Badang Italic;" +
        "Badly Stamped;" +
        "Bebas;" +
        "Hells Rider Decay;" +
        "The Inbox St;" +
        "Zigzageo;" +
        "Impact=impact,chicago;",
    fontsize_formats: "8pt 10p 12pt 14pt 16pt 18pt 20pt 22pt 24pt 26pt 28pt 30pt 32pt 34pt 36pt 48pt 60pt",
    selector: '.editor',

    plugins: 'print preview fullpage paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    imagetools_cors_hosts: ['picsum.photos'],
    menubar: 'file edit view insert format tools table help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview print | insertfile image media link anchor codesample | ltr rtl',
    toolbar_sticky: true,
    autosave_ask_before_unload: true,
    autosave_interval: "30s",
    autosave_prefix: "{path}{query}-{id}-",
    autosave_restore_when_empty: false,
    autosave_retention: "2m",
    image_advtab: true,
    branding: false,
    importcss_append: true,
    file_picker_types: 'file image media',
    file_picker_callback: function (callback, value, meta) {
        /* Provide file and text for the link dialog */
        if (meta.filetype === 'file') {
            callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
        }

        /* Provide image and alt text for the image dialog */
        if (meta.filetype === 'image') {
            callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
        }

        /* Provide alternative source and posted for the media dialog */
        if (meta.filetype === 'media') {
            // callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.google.com/logos/google.jpg' });

            // Trigger click on file element
            jQuery("#fileupload").trigger("click");
            $("#fileupload").unbind('change');
            // File selection
            jQuery("#fileupload").on("change", function () {
                var file = this.files[0];
                var reader = new FileReader();

                // FormData
                var fd = new FormData();
                var files = file;
                fd.append("file", files);
                fd.append('filetype', meta.filetype);

                var filename = "";

                // AJAX
                //var url = '(@BaseURL)/elearning/File/EditorUpload';
                var url = editorUploadUrl;
                //console.log('url - ', url);

                jQuery.ajax({
                    url: url,
                    type: "post",
                    data: fd,
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (response) {
                        //console.log('filename-', response);
                        filename = response.location;
                    }
                });

                reader.onload = function (e) {
                    callback(filename);
                };
                reader.readAsDataURL(file);
            });
        }
    },

    //  width: 800,
    height: 800,
    image_caption: true,
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
    noneditable_noneditable_class: "mceNonEditable",
    toolbar_drawer: 'sliding',
    contextmenu: "link image imagetools table",
    //images_upload_url: '@BaseURL/elearning/File/EditorUpload',
    //images_upload_base_path: '@BaseURL/eLearning',
    images_upload_url: editorUploadUrl,
    images_upload_base_path: basePath,
    images_upload_credentials: true,
    relative_urls: false,
    remove_script_host: false,
    convert_urls: true,

    init_instance_callback: function (editor) {
        editor.on('BeforeSetContent', function (e) {
            //console.log(e.content);

            if (e.content.indexOf("video ") > 0) {
                if (e.content.indexOf(".pdf") > 0) {
                    e.content = e.content.replace("<video", "<div");

                    var width = e.content.match("width=\"(.*)\" height");
                    var height = e.content.match("height=\"(.*)\" controls");
                    //console.log('width - ', width, ' height - ', height);

                    e.content = e.content.replace("controls=\"controls\"", "");

                    e.content = e.content.replace("<source", "<object");
                    e.content = e.content.replace("src=", "data=");
                    //console.log('middle - ', e.content);

                    e.content = e.content.replace("</video", "</div");
                    e.content = e.content.replace(" />", "type=\"application/pdf\" width=\"" + width[1] + "\" height=\"" + height[1] + "\"  ></object>");
                }

                //console.log('after - ', e.content);
            }
        });
    },
});