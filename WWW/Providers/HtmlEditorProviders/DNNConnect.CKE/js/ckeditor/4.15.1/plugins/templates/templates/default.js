
CKEDITOR.addTemplates("default", {
    imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath("templates") + "templates/images/"),
    templates: [{
        title: "Image and Title", image: "template1.gif",
        description: "One main image with a title and text that surround the image.",
        html: '\x3ch3\x3e\x3cimg src\x3d" " alt\x3d"" style\x3d"margin-right: 10px" height\x3d"100" width\x3d"100" align\x3d"left" /\x3eType the title here\x3c/h3\x3e\x3cp\x3eType the text here\x3c/p\x3e'
    },
    {
        title: "Box Trái",
        image: "template2.gif",
        description: "A template that defines two columns, each one with a title, and some text.",
        html: '<div class="boxnhungtrongbai-trai">' +
            '<p>Nội dung</p></div>'
    },
    {
        title: "Box Phải",
        image: "template2.gif",
        description: "A template that defines two columns, each one with a title, and some text.",
        html: '<div class="boxnhungtrongbai-phai">' +
            '<p>Nội dung</p></div>'
    },
    {
        title: "Strange Template",
        image: "template2.gif",
        description: "A template that defines two columns, each one with a title, and some text.",
        html: '\x3ctable cellspacing\x3d"0" cellpadding\x3d"0" style\x3d"width:100%" border\x3d"0"\x3e\x3ctr\x3e\x3ctd style\x3d"width:50%"\x3e\x3ch3\x3eTitle 1\x3c/h3\x3e\x3c/td\x3e\x3ctd\x3e\x3c/td\x3e\x3ctd style\x3d"width:50%"\x3e\x3ch3\x3eTitle 2\x3c/h3\x3e\x3c/td\x3e\x3c/tr\x3e\x3ctr\x3e\x3ctd\x3eText 1\x3c/td\x3e\x3ctd\x3e\x3c/td\x3e\x3ctd\x3eText 2\x3c/td\x3e\x3c/tr\x3e\x3c/table\x3e\x3cp\x3eMore text goes here.\x3c/p\x3e'
    }, {
        title: "Text and Table", image: "template3.gif", description: "A title with some text and a table.",
        html: '\x3cdiv style\x3d"width: 80%"\x3e\x3ch3\x3eTitle goes here\x3c/h3\x3e\x3ctable style\x3d"width:150px;float: right" cellspacing\x3d"0" cellpadding\x3d"0" border\x3d"1"\x3e\x3ccaption style\x3d"border:solid 1px black"\x3e\x3cstrong\x3eTable title\x3c/strong\x3e\x3c/caption\x3e\x3ctr\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3c/tr\x3e\x3ctr\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3c/tr\x3e\x3ctr\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3ctd\x3e\x26nbsp;\x3c/td\x3e\x3c/tr\x3e\x3c/table\x3e\x3cp\x3eType the text here\x3c/p\x3e\x3c/div\x3e'
    },
    {
        title: "Box nhúng 1", image: "box2.png", description: "Full - Nền Vàng Nhạt - Viền trái",
        html: '<div style="background-color: #f9f879; border-left: 6px solid #ff0000;margin:20px;font-size:0.9em;padding:4px 10px;box-shadow: 1px 0px 2px 1px rgba(0, 0, 0, 0.16), 0 2px 10px 0 rgba(0, 0, 0, 0.12);">' +
            '<p>Nội dung</p></div>'
    },
    {
        title: "Box nhúng 2", image: "box1.png", description: "Full - Nền Vàng Nhạt",
        html: '<div style="background-color: #fef5c4;border:solid 1px #fbe4a2; margin:10px;padding:4px 10px; font-size:0.9em;">' +
            '<p>Nội dung</p></div>'
    },
    {
        title: "Box nhúng 3", image: "box3.png", description: "Full - Nền Nhạt",
        html: '<div style="background: #eee;border-top: 2px solid #009cd7;padding: 15px 20px 5px;">' +
            '<p>Nội dung</p></div>'
    }
    ]
});