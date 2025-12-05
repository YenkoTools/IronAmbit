# Iron Ambit Web Application

A modern web application built with Astro, featuring Tailwind CSS, React components, and MDX support.

## Features

- ⚡ **Astro** - Ultra-fast static site generation with partial hydration
- 🎨 **Tailwind CSS** - Utility-first CSS framework for rapid UI development
- ⚛️ **React** - Interactive components with optimal hydration strategies
- 📝 **MDX** - Markdown with JSX component support
- 📱 **Responsive Layout** - Mobile-first design with header, sidebar, and footer
- 🎯 **TypeScript** - Type-safe development experience

## Layout Structure

The application includes a fully responsive layout with:

- **Header** - Navigation bar with branding and main menu
- **Sidebar** - Navigation links for easy site navigation
- **Main Content** - Flexible content area for pages
- **Footer** - Copyright information for Hallcrest Engineering, 2025

## 🚀 Project Structure

```text
Client/
├── src/
│   ├── components/
│   │   ├── Header.astro       # Site header
│   │   ├── Sidebar.astro      # Navigation sidebar
│   │   ├── Footer.astro       # Site footer
│   │   └── Counter.tsx        # Example React component
│   ├── layouts/
│   │   └── Layout.astro       # Main layout template
│   ├── pages/
│   │   ├── index.astro        # Home page
│   │   ├── about.astro        # About page
│   │   └── docs/
│   │       ├── index.astro    # Documentation index
│   │       └── getting-started.mdx  # MDX documentation
│   └── styles/
│       └── global.css         # Global Tailwind CSS
├── astro.config.mjs           # Astro configuration
├── tailwind.config.js         # Tailwind configuration
└── package.json
```

## 🧞 Commands

All commands are run from the root of the project, from a terminal:

| Command                   | Action                                           |
| :------------------------ | :----------------------------------------------- |
| `npm install`             | Installs dependencies                            |
| `npm run dev`             | Starts local dev server at `localhost:4321`      |
| `npm run build`           | Build your production site to `./dist/`          |
| `npm run preview`         | Preview your build locally, before deploying     |
| `npm run astro ...`       | Run CLI commands like `astro add`, `astro check` |
| `npm run astro -- --help` | Get help using the Astro CLI                     |

## Customization

### Modifying the Layout

Edit the layout components in `src/components/`:

- `Header.astro` - Update branding and navigation
- `Sidebar.astro` - Modify navigation links
- `Footer.astro` - Customize footer content

### Adding Pages

Create new `.astro` or `.mdx` files in `src/pages/`

### Styling

All components use Tailwind CSS utility classes. Customize the theme in `tailwind.config.js`.

## 👀 Learn More

- [Astro Documentation](https://docs.astro.build)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [React Documentation](https://react.dev)

---

© Hallcrest Engineering, 2025
