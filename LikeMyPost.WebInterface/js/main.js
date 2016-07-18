(function myfunction() {


    $("#logout").on("click", logout);
    $("#CommentSubmit").on("click", addComment);
    $("#ListOfPosts").on("click", ".vote", VotePost);
    $(".post-opened").on("click", ".vote", VotePost);
    $(".comments").on("click", ".vote", VoteComment);
    $(".comments").on("click", ".delete-comment", deleteComment);
    $("#ListOfPosts").on("click", ".delete-post", deletePost);
    $("#ListOfUsers").on("click", ".giveModerator", GiveModerator);
    $("#ListOfUsers").on("click", ".removeModerator", RemoveModerator);


    $(document).ready(function () {
        $(document).on('click', '#login_show', function () {
            $('#Login').slideToggle(250, function () {
                $(window).scrollTop($("#Login").offset().top);
            });
        });
    });

    function GiveModerator() {
        var id = $(this).closest("[data-user-id]", ".users").data("user-id");
        var fd = new FormData();
        fd.append('type', 'Add');
        fd.append('id', id);
        $.ajax({
            url: '/actions/ModeratorRights.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var btn = $('[data-user-id=' + id + ']  .giveModerator');
            if (success == true) {
                btn.removeClass("giveModerator").addClass("removeModerator");
                btn.text("Remove Moderator");
            }
        })
    }
    
    function RemoveModerator() {
        var id = $(this).closest("[data-user-id]", ".users").data("user-id");
        var fd = new FormData();
        fd.append('type', 'Remove');
        fd.append('id', id);
        $.ajax({
            url: '/actions/ModeratorRights.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var btn = $('[data-user-id=' + id + ']  .removeModerator');
            if (success == true) {
                btn.removeClass("removeModerator").addClass("giveModerator");
                btn.text("Give Moderator");
            }
        })
    }

    $('textarea').autosize();
    $("#LoginForm").validate({
        messages: {
            "email": {
                required: "<br/><span style='color:red;'>Empty</a>",
                email: "<br/><span style='color:red;'> Incorrect</a>"
            },
            "password": {
                required: "<br/><span style='color:red;'>Обязательное поле!</a>",
                minlength: "<br/><span style='color:red;'> Little!</a>",
            }
        },
        submitHandler: function (form) {
            var login_data = new FormData($('#LoginForm')[0]);
            $.ajax({
                url: '/actions/Login.cshtml',
                type: 'post',
                cache: false,
                contentType: false,
                processData: false,
                data: login_data,
            })
            .success(function (data) {
                data = JSON.parse(data);
                var success = data["success"];
                switch (success) {
                    case -1:
                        $("#LoginError").removeClass("hide");
                        $("#LoginError").text("Database error");
                        break;
                    case 1:
                        location.reload(true);
                        break;
                    case 0:
                        $("#LoginError").removeClass("hide");
                        $("#LoginError").text("Can't login with this mail or password");
                        break;
                    default:
                        $("#LoginError").removeClass("hide");
                        $("#LoginError").text("Unknown error");
                }
            })

        }

    });
    $("#RegisterForm").validate({
        messages: {
            "email": {
                required: "<br/><span style='color:red;'>Empty</a>",
                email: "<br/><span style='color:red;'> Incorrect</a>"
            },
            "password": {
                required: "<br/><span style='color:red;'>Обязательное поле!</a>",
                minlength: "<br/><span style='color:red;'> Little!</a>",
            },
            "username": {
                required: "<br/><span style='color:red;'>Обязательное поле!</a>",
                minlength: "<br/><span style='color:red;'> Little!</a>",
            }
        },
        submitHandler: function (form) {
            var register_data = new FormData($('#RegisterForm')[0]);
            $.ajax({
                url: '/actions/Register',
                type: 'post',
                cache: false,
                contentType: false,
                processData: false,
                data: register_data,
            })
            .success(function (data) {
                data = JSON.parse(data);
                var success = data["success"];
                switch (success) {
                    case 0:
                        $("#RegisterError").removeClass("hide");
                        $("#RegisterError").text("Database error");
                        break;
                    case 1:
                        location.reload(true);
                        break;
                    case -1:
                        $("#RegisterError").removeClass("hide");
                        $("#RegisterError").text("User with this username or mail already exists");
                        break;
                    default:
                        $("#RegisterError").removeClass("hide");
                        $("#RegisterError").text("Unknown error");
                }
            })

        }

    });    
    $("#AddPostForm").validate({
        messages: {
            "title": {
                required: "<br/><span style='color:red;'>Обязательное поле!</a>",
                maxlength: "<br/><span style='color:red;'>Максимальное кол-во символов 100 единиц!</a>",
                minlength: "<br/><span style='color:red;'>Слишком маленький заголовок!</a>"
            },
            "text": { required: "<br/><span style='color:red;'>Обязательное поле!</a>" }
        },
        submitHandler: function (form) {     
            var post_data = new FormData($('#AddPostForm')[0]);
            post_data.append('image', $('#image-upload')[0].files[0]);
            var sizeInBytes = ($('#image-upload')[0].files[0].size);
            var sizeInMB = (sizeInBytes / (1024 * 1024)).toFixed(2);
            if (sizeInMB > 4)
            {
                $("#AddPostError").removeClass("hide");
                $("#AddPostError").text("Image is too big");
                return;
            }
            $.ajax({
                url: '/actions/AddPost',
                type: 'post',
                cache: false,
                contentType: false,
                processData: false,
                data: post_data,
            })
            .success(function (data) {

                data = JSON.parse(data);
                var success = data["success"];
                switch (success) {
                    case -1:
                        $("#AddPostError").removeClass("hide");
                        $("#AddPostError").text("Server error");
                        break;
                    case 1:
                        location.assign("/index")
                        break;
                    case 0:
                        $("#AddPostError").removeClass("hide");
                        $("#AddPostError").text("Post validation fails");
                        break;
                    default:
                        $("#AddPostError").removeClass("hide");
                        $("#AddPostError").text("Unknown error");
                }
            })

        }

    });


    function deletePost() {

        var id = $(this).closest("[data-post-id]", "#ListOfPosts").data("post-id");
        var fd = new FormData();
        fd.append('type', 'Post');
        fd.append('id', id);
        $.ajax({
            url: '/actions/Delete.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var counter = $('[data-post-id=' + id + ']  .counter');
            var button = $('[data-post-id=' + id + ']  .delete-post');
            if (success == true) {
                counter.empty();
                button.remove();
            }
        })
    }

    function deleteComment() {
        var id = $(this).closest("[data-comment-id]", ".comments").data("comment-id");
        var fd = new FormData();
        fd.append('type', 'Comment');
        fd.append('id', id);
        $.ajax({
            url: '/actions/Delete.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var counter = $('[data-post-id=' + id + ']  .counter');
            if (success == true) {
                counter.empty();
            }
        })
    }

    function VotePost() {
        var id = $(this).closest("[data-post-id]", "#ListOfPosts").data("post-id");
        var fd = new FormData();
        fd.append('type', 'postVote');
        fd.append('id', id);
        $.ajax({
            url: '/actions/Vote.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var value = data["value"];
            var like = $('[data-post-id=' + id + ']  .vote');
            console.log(like);
            if(success == true)
            {
                $('[data-post-id=' + id + ']  .votes').text(value);
                
                if (like.hasClass("voted"))
                {
                    like.text("Like!");
                    like.removeClass("voted");
                }
                else {
                    like.text("DisLike!");
                    like.addClass("voted");
                }
                
            }
        })
    }

    function VoteComment() {     
        var id = $(this).closest("[data-comment-id]", ".comments").data("comment-id");
        var fd = new FormData();
        fd.append('type', 'commentVote');
        fd.append('id', id);
        $.ajax({
            url: '/actions/Vote.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: fd,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            var value = data["value"];
            var like = $('[data-comment-id=' + id + ']  .vote');
            console.log(like);
            if (success == true) {
                $('[data-comment-id=' + id + ']  .votes').text(value);

                if (like.hasClass("voted")) {
                    like.text("Like!");
                    like.removeClass("voted");
                }
                else {
                    like.text("DisLike!");
                    like.addClass("voted");
                }

            }
        })
    }

   
    function showNew() {
        $.ajax({
            url: '/actions/GetNew',
            type: 'post',
            cache: false
        })
        .success(function (data) {           
            document.title = 'Свежее!';
            $("#ListOfPosts").empty();
            $("#ListOfPosts").append(data);
        })
    }



    function login(e) {
        e.preventDefault();
        var login_data = new FormData($('#LoginForm')[0]);
        $.ajax({
            url: '/actions/Login.cshtml',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: login_data,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            switch (success) {
                case 0:
                    $("#LoginError").removeClass("hide");
                    $("#LoginError").text("Database error");
                    break;
                case -1:
                    location.reload(true);
                    break;
                case 1:
                    $("#LoginError").removeClass("hide");
                    $("#LoginError").text("Can't login with this mail or password");
                    break;
                default:
                    $("#LoginError").removeClass("hide");
                    $("#LoginError").text("Unknown error");
            }
        })
    }

    function logout(e) {
        e.preventDefault();
        $.ajax({
            url: '/actions/Logout.cshtml',
            type: 'post',
        })
        .success(function (data) {
            location.reload(true);
        })
    }

    function register(e) {
        e.preventDefault();
        var register_data = new FormData($('#RegisterForm')[0]);
        $.ajax({
            url: '/actions/Register',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: register_data,
        })
        .success(function (data) {
            data = JSON.parse(data);
            var success = data["success"];
            switch (success) {
                case 0:
                    $("#RegisterError").removeClass("hide");
                    $("#RegisterError").text("Database error");
                    break;
                case -1:
                    location.reload(true);
                    break;
                case 1:
                    $("#RegisterError").removeClass("hide");
                    $("#RegisterError").text("User with this username or mail already exists");
                    break;
                default:
                    $("#RegisterError").removeClass("hide");
                    $("#RegisterError").text("Unknown error");
            }
        })
    }

    function addComment(e) {
        e.preventDefault();
        var comment_data = new FormData($('#AddComment')[0]);
        $.ajax({
            url: '/actions/AddComment',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: comment_data,
        })
        .success(function (data) {
            $("#comment-area").val("");
            $(".comments ul").append(data);
        })
    }

}())