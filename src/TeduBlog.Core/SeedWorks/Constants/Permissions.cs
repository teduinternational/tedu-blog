using System.ComponentModel;

namespace TeduBlog.Core.SeedWorks.Constants
{
    public static class Permissions
    {
        public static class Dashboard
        {
            [Description("Xem dashboard")]
            public const string View = "Permissions.Dashboard.View";
        }
        public static class Roles
        {
            [Description("Xem quyền")]
            public const string View = "Permissions.Roles.View";
            [Description("Tạo mới quyền")]
            public const string Create = "Permissions.Roles.Create";
            [Description("Sửa quyền")]
            public const string Edit = "Permissions.Roles.Edit";
            [Description("Xóa quyền")]
            public const string Delete = "Permissions.Roles.Delete";
        }
        public static class Users
        {
            [Description("Xem người dùng")]
            public const string View = "Permissions.Users.View";
            [Description("Tạo người dùng")]
            public const string Create = "Permissions.Users.Create";
            [Description("Sửa người dùng")]
            public const string Edit = "Permissions.Users.Edit";
            [Description("Xóa người dùng")]
            public const string Delete = "Permissions.Users.Delete";
        }
        public static class PostCategories
        {
            [Description("Xem danh mục bài viết")]
            public const string View = "Permissions.PostCategories.View";
            [Description("Tạo danh mục bài viết")]
            public const string Create = "Permissions.PostCategories.Create";
            [Description("Sửa danh mục bài viết")]
            public const string Edit = "Permissions.PostCategories.Edit";
            [Description("Xóa danh mục bài viết")]
            public const string Delete = "Permissions.UsPostCategoriesers.Delete";
        }
        public static class Posts
        {
            [Description("Xem bài viết")]
            public const string View = "Permissions.Posts.View";
            [Description("Tạo bài viết")]
            public const string Create = "Permissions.Posts.Create";
            [Description("Sửa bài viết")]
            public const string Edit = "Permissions.Posts.Edit";
            [Description("Xóa bài viết")]
            public const string Delete = "Permissions.Posts.Delete";
            [Description("Duyệt bài viết")]
            public const string Approve = "Permissions.Posts.Approve";
        }

        public static class Series
        {
            [Description("Xem loạt bài")]
            public const string View = "Permissions.Series.View";
            [Description("Tạo loạt bài")]
            public const string Create = "Permissions.Series.Create";
            [Description("Sửa loạt bài")]
            public const string Edit = "Permissions.Series.Edit";
            [Description("Xóa loạt bài")]
            public const string Delete = "Permissions.Series.Delete";
        }

        public static class Royalty
        {
            [Description("Xem nhuận bút")]
            public const string View = "Permissions.Royalty.View";
            [Description("Trả nhuận bút")]
            public const string Pay = "Permissions.Royalty.Pay";
        }
    }
}
