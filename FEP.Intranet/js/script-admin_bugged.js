(function($) {
    "use strict";
    var wind = $(window);
    $(function() {
        var akpk = {
            smoothScroll: function() {
                $('.navbar-nav a, #mobile-nav a, .scrollto').on('click', function() {
                    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                        var target = $(this.hash);
                        if (target.length) {
                            var top_space = 0;

                            if ($('#header').length) {
                                top_space = $('#header').outerHeight();

                                if (!$('#header').hasClass('header-fixed')) {
                                    top_space = top_space - 20;
                                }
                            }

                            $('html, body').animate({
                                scrollTop: target.offset().top - top_space
                            }, 1500, 'easeInOutExpo');

                            if ($(this).parents('.navbar-nav').length) {
                                $('.navbar-nav .menu-active').removeClass('menu-active');
                                $(this).closest('li').addClass('menu-active');
                            }

                            if ($('body').hasClass('mobile-nav-active')) {
                                $('body').removeClass('mobile-nav-active');
                                $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                                $('#mobile-body-overly').fadeOut();
                            }
                            return false;
                        }
                    }
                });

            },

            toggleMainMenu: function() {
                $("#menu-collapse").on('show.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").addClass("open");
                    $(".header__content").addClass('has-border');
                    $(".header-container").css({
                        'height': '128',
                        'transition': 'all 0.4s ease-out 0s'
                    });

                }).on('hide.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").removeClass("open");
                    $(".header__content").removeClass('has-border');
                    $(".header-container").css({
                        'height': '64',
                        'transition': 'all 0.4s ease-in 0s'
                    });
                });

                if (wind.width() >= 991) {
                    $('#menu-collapse').collapse('show');
                }



            },

            //configure the select2 custom plugin
            selectOption: function() {
                $('.select2').select2();

                $('.select2-area').select2({
                    placeholder: 'Select Area'
                });

                //Multiple select
                $('.js-example-basic-multiple').select2({
                    placeholder: 'Select an option'
                });

            },

            dateSelect: function() {
                $(".dateSelect").flatpickr({
                    // mode: "range",
                    dateFormat: "j M y",
                    wrap: true
                });

                $(".timeSelect").flatpickr({
                    enableTime: true,
                    noCalendar: true,
                    dateFormat: "H:i"
                });
            },

            //list file browse input
            updateList: function() {
                var input = document.getElementById('inputfile');
                var output = document.getElementById('fileList');

                $('#inputfile').change(function() {
                    output.innerHTML = '<ul>';
                    for (var i = 0; i < input.files.length; ++i) {
                        output.innerHTML += '<li>' + input.files.item(i).name + '</li>';
                    }
                    output.innerHTML += '</ul>';
                    $('#fileList').wrapAll("<div class='list-file-container'></div>");

                });
            },

            filter: function() {
                $('body').on('click', '.filter.dropdown .cycle-element', function(e) {
                    $(this).parent().toggleClass('open');
                });
                $('body').on('click', function(e) {
                    if (!$('.filter.dropdown').is(e.target) && $('.filter.dropdown').has(e.target).length === 0 && $('.open').has(e.target).length === 0) {
                        $('.filter.dropdown').removeClass('open');
                    }
                });
            },

            //select event category list
            selectCategoryEvent: function() {
                var boxEvent = $(".box-contain .card");

                boxEvent.click(function() {
                    $(".box-contain .card").removeClass('active');
                    $(this).addClass('active');
                });

            },

            //For height Action Log Media Interview to trigger scroll
            actionLogHeight: function() {
                var formContainerH = $(".form-container").innerHeight();
                //alert (formContainer);
                $(".action-log .ps").css({
                    'max-height': formContainerH
                })


            }

            

        };

        akpk.smoothScroll();
        akpk.toggleMainMenu();
        akpk.selectOption();
        akpk.dateSelect();
        akpk.updateList();
        akpk.filter();
        akpk.selectCategoryEvent();
        akpk.actionLogHeight();
       


    });


});