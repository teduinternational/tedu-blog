import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Trang chủ',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    badge: {
      color: 'info',
      text: 'NEW',
    },
  },
  {
    name: 'Nội dung',
    url: '/content',
    iconComponent: { name: 'cil-puzzle' },
    children: [
      {
        name: 'Danh mục',
        url: '/content/post-categories',
      },
      {
        name: 'Bài viết',
        url: '/content/posts',
      },
      {
        name: 'Loạt bài',
        url: '/content/series',
      },
      {
        name: 'Nhuận bút',
        url: '/content/royalty',
      }
    ],
  },

  {
    name: 'Hệ thống',
    url: '/system',
    iconComponent: { name: 'cil-notes' },
    children: [
      {
        name: 'Quyền',
        url: '/system/roles',
      },
      {
        name: 'Người dùng',
        url: '/system/users',
      }
    ],
  },
];
