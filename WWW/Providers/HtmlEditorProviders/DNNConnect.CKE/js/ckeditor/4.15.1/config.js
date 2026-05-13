/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbar = [
        { name: 'styles', items: ['Source', 'Style', 'Format'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'tools', items: ['Maximize'] },
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
        { name: 'document', items: ['Templates','Youtube'] },
        { name: 'clipboard', items: ['Cut', 'Copy','Undo', 'Redo'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },
        { name: 'links', items: ['Link', 'Unlink', 'dangkydo', 'dangkydoen'] },
        { name: 'insert', items: ['Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak',  'easyimage'] },
    ];
    //----
    config.skin = 'bootstrapck';
    // Set the most common block elements.
    config.format_tags = 'p;h1;h2;h3;pre';
    // Simplify the dialog windows.
    //config.removeDialogTabs = 'image:advanced;link:advanced';
    config.extraPlugins = 'youtube,oembed,filetools,uploadwidget,dangkydo,dangkydoen,chuky,chukyen';
    config.removePlugins = 'easyimage';
    config.youtube_width = '744';
    config.youtube_responsive = true;
    config.youtube_related = false;
    config.youtube_autoplay = false;
    config.youtube_height = '480';
    config.language = 'vi';
    config.height = '800px';
    config.forcePasteAsPlainText = false;
    config.pasteFromWordRemoveStyles = true;
    config.pasteFromWordRemoveFontStyles = true;
    config.pasteFromWordRemoveStyles = true;
    config.templates_replaceContent = false;
    //=================================================
    //--
    config.allowedContent = true;
    config.resize_enabled = false;
    //Word count
    config.wordcount = {

        // Whether or not you want to show the Paragraphs Count
        showParagraphs: true,

        // Whether or not you want to show the Word Count
        showWordCount: true,

        // Whether or not you want to show the Char Count
        showCharCount: true,

        // Whether or not you want to count Spaces as Chars
        countSpacesAsChars: false,

        // Whether or not to include Html chars in the Char Count
        countHTML: false,

        // Maximum allowed Word Count, -1 is default for unlimited
        maxWordCount: -1,

        // Maximum allowed Char Count, -1 is default for unlimited
        maxCharCount: -1,

        // Add filter to add or remove element before counting (see CKEDITOR.htmlParser.filter), Default value : null (no filter)
        filter: new CKEDITOR.htmlParser.filter({
            elements: {
                div: function (element) {
                    if (element.attributes.class == 'mediaembed') {
                        return false;
                    }
                }
            }
        })
    };
};
