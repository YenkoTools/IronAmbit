# Iron Ambit Color Palette Guide

This color palette is extracted from the Iron Ambit logo (`ironambit.svg`) and configured for use with Tailwind CSS v4.

## Color Schemes

### Primary (Deep Navy Blue)
The primary color is the rich navy blue from the logo text and darker elements.

**Usage:**
- Headers and navigation
- Primary buttons and CTAs
- Text headings
- Brand elements

**Examples:**
```html
<!-- Backgrounds -->
<div class="bg-primary-800">Dark navy background</div>
<div class="bg-primary-600">Medium navy background</div>

<!-- Text -->
<h1 class="text-primary-900">Dark navy heading</h1>
<p class="text-primary-700">Navy text</p>

<!-- Borders -->
<div class="border-2 border-primary-500">Navy border</div>

<!-- Buttons -->
<button class="bg-primary-700 hover:bg-primary-800 text-white">Primary Button</button>
```

### Secondary (Warm Cream/Beige)
The secondary color is the warm cream/beige from the logo background.

**Usage:**
- Page backgrounds
- Card backgrounds
- Section dividers
- Soft accents
- Alternative backgrounds

**Examples:**
```html
<!-- Backgrounds -->
<body class="bg-secondary-300">Warm cream background</body>
<div class="bg-secondary-200">Light cream section</div>

<!-- Cards -->
<div class="bg-secondary-100 border border-secondary-400">
  Card with cream background
</div>

<!-- Subtle highlights -->
<div class="hover:bg-secondary-200">Hover effect</div>
```

### Accent (Soft Gold/Amber)
The accent color provides warmth and highlights.

**Usage:**
- Call-to-action elements
- Highlights and emphasis
- Success states
- Interactive elements
- Icons and decorative elements

**Examples:**
```html
<!-- Highlighted elements -->
<span class="bg-accent-400 text-accent-900 px-2 py-1 rounded">Badge</span>

<!-- Buttons -->
<button class="bg-accent-500 hover:bg-accent-600 text-white">Action Button</button>

<!-- Icons -->
<svg class="text-accent-500">...</svg>

<!-- Borders for emphasis -->
<div class="border-l-4 border-accent-500">Highlighted content</div>
```

### Neutral (Warm Grays)
Neutral colors adjusted to complement the warm palette.

**Usage:**
- Body text
- Borders
- Dividers
- Subtle backgrounds
- Disabled states

**Examples:**
```html
<!-- Text -->
<p class="text-neutral-700">Body text</p>
<p class="text-neutral-500">Secondary text</p>
<p class="text-neutral-400">Muted text</p>

<!-- Borders -->
<div class="border border-neutral-300">Card with border</div>

<!-- Dividers -->
<hr class="border-neutral-200" />

<!-- Disabled states -->
<button class="bg-neutral-300 text-neutral-500 cursor-not-allowed" disabled>
  Disabled Button
</button>
```

## Color Combinations

### Header/Navigation
```html
<header class="bg-primary-800 text-white">
  <nav class="hover:bg-primary-700 transition-colors">...</nav>
</header>
```

### Hero Section
```html
<section class="bg-secondary-200">
  <h1 class="text-primary-900">Welcome to Iron Ambit</h1>
  <p class="text-neutral-700">Description text</p>
  <button class="bg-accent-500 hover:bg-accent-600 text-white">Get Started</button>
</section>
```

### Cards
```html
<div class="bg-white border border-secondary-400 hover:border-accent-500 transition-colors">
  <h3 class="text-primary-800">Card Title</h3>
  <p class="text-neutral-600">Card content</p>
</div>
```

### Sidebar
```html
<aside class="bg-secondary-100 border-r border-secondary-300">
  <a href="#" class="text-primary-700 hover:bg-secondary-200 hover:text-primary-900">
    Navigation Link
  </a>
</aside>
```

### Footer
```html
<footer class="bg-primary-900 text-secondary-100">
  <p>&copy; Hallcrest Engineering, 2025</p>
  <a href="#" class="text-secondary-300 hover:text-accent-400">Link</a>
</footer>
```

## Gradients

```html
<!-- Primary gradient -->
<div class="bg-gradient-to-r from-primary-700 to-primary-900">...</div>

<!-- Warm gradient -->
<div class="bg-gradient-to-br from-secondary-200 to-accent-200">...</div>

<!-- Subtle gradient -->
<div class="bg-gradient-to-b from-white to-secondary-100">...</div>
```

## Accessibility Tips

1. **Text Contrast**: Always ensure sufficient contrast between text and background
   - Use `text-primary-900` on light backgrounds
   - Use `text-secondary-100` on dark backgrounds

2. **Interactive Elements**: Provide clear hover and focus states
   ```html
   <button class="bg-primary-700 hover:bg-primary-800 focus:ring-2 focus:ring-accent-500">
     Button
   </button>
   ```

3. **Dark Mode**: Consider adding dark mode variants
   ```html
   <div class="bg-white dark:bg-primary-900 text-primary-900 dark:text-secondary-100">
     Content
   </div>
   ```

## Quick Reference

| Color | Main Usage | Example Classes |
|-------|-----------|-----------------|
| Primary | Brand, headers, buttons | `bg-primary-800`, `text-primary-700` |
| Secondary | Backgrounds, cards | `bg-secondary-200`, `border-secondary-400` |
| Accent | Highlights, CTAs | `bg-accent-500`, `text-accent-600` |
| Neutral | Text, borders | `text-neutral-700`, `border-neutral-300` |

## Updating Components

To use the new palette, simply replace existing Tailwind color classes:

**Before:**
```html
<header class="bg-blue-600">...</header>
```

**After:**
```html
<header class="bg-primary-800">...</header>
```

The custom colors are now available throughout your application and can be used with all Tailwind utilities (backgrounds, text, borders, shadows, etc.).
