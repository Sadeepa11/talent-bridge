---
name: Recruitment Intelligence System
colors:
  surface: '#f7f9fb'
  surface-dim: '#d8dadc'
  surface-bright: '#f7f9fb'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f4f6'
  surface-container: '#eceef0'
  surface-container-high: '#e6e8ea'
  surface-container-highest: '#e0e3e5'
  on-surface: '#191c1e'
  on-surface-variant: '#41474e'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#72787f'
  outline-variant: '#c1c7cf'
  surface-tint: '#2f6388'
  primary: '#003b5a'
  on-primary: '#ffffff'
  primary-container: '#1a5276'
  on-primary-container: '#94c5ee'
  inverse-primary: '#9bccf6'
  secondary: '#006397'
  on-secondary: '#ffffff'
  secondary-container: '#71c0fe'
  on-secondary-container: '#004d77'
  tertiary: '#00401e'
  on-tertiary: '#ffffff'
  tertiary-container: '#005a2c'
  on-tertiary-container: '#58d683'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#cbe6ff'
  primary-fixed-dim: '#9bccf6'
  on-primary-fixed: '#001e30'
  on-primary-fixed-variant: '#0e4b6e'
  secondary-fixed: '#cce5ff'
  secondary-fixed-dim: '#92ccff'
  on-secondary-fixed: '#001d31'
  on-secondary-fixed-variant: '#004b73'
  tertiary-fixed: '#7efba4'
  tertiary-fixed-dim: '#61de8a'
  on-tertiary-fixed: '#00210c'
  on-tertiary-fixed-variant: '#005228'
  background: '#f7f9fb'
  on-background: '#191c1e'
  surface-variant: '#e0e3e5'
typography:
  headline-lg:
    fontFamily: Inter
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.02em
  headline-md:
    fontFamily: Inter
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.01em
  headline-sm:
    fontFamily: Inter
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  body-sm:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 18px
  label-md:
    fontFamily: Inter
    fontSize: 13px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.05em
  code:
    fontFamily: Inter
    fontSize: 13px
    fontWeight: '500'
    lineHeight: 16px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  sidebar_width: 260px
  topbar_height: 64px
  gutter: 24px
  margin-desktop: 32px
  container-max: 1440px
---

## Brand & Style
The design system is engineered for a high-utility B2B environment, focusing on professional reliability and data density. The aesthetic follows a **Corporate / Modern** approach with a heavy emphasis on structural clarity and systematic organization. 

The target audience consists of HR directors, recruitment specialists, and data analysts who require a calm, "no-fuss" interface to process large volumes of talent data. The UI evokes a sense of authority and precision through a disciplined use of whitespace, a rigid grid, and a sober color palette that prioritizes readability over decoration.

## Colors
The color strategy utilizes a deep Navy (#1A5276) as the primary anchor to instill trust and corporate stability. The Accent Blue (#2E86C1) is reserved for interactive elements like primary buttons and active states. 

A critical functional component of this design system is the **Access Status Palette**. These colors are used exclusively to denote data accessibility:
- **Limited (Amber):** Warning/Partial access.
- **Full (Green):** Unrestricted access.
- **Expiring (Red):** Urgent action required.
- **Reserved (Grey):** Standard/Baseline access.

Backgrounds are kept at a very cool, desaturated #F7F9FB to minimize eye strain during long working sessions.

## Typography
This design system utilizes **Inter** for its exceptional legibility in data-heavy interfaces. The typographic scale is compact to maximize information density. 

- **Headlines:** Use tighter letter spacing and bold weights to provide clear hierarchy.
- **Body Text:** The 14px `body-md` is the workhorse for data tables and profile descriptions.
- **Labels:** Small caps or increased letter spacing are used for secondary metadata and table headers to distinguish them from actionable data.

## Layout & Spacing
The layout follows a **Desktop-First Fixed Grid** model, optimized for 1440px resolution. 

1.  **Persistent Sidebar:** A 260px fixed-width navigation bar on the left ensures critical tools are always accessible.
2.  **Top Navigation:** A 64px tall bar containing search, notifications, and user profile stays pinned to the top of the viewport.
3.  **Main Content Area:** A fluid container with a maximum width of 1440px, utilizing a 12-column grid system with 24px gutters.
4.  **Data Density:** Vertical rhythm is tight (8px increments), allowing more rows of data to be visible without scrolling.

## Elevation & Depth
The design system uses **Low-contrast outlines** combined with very subtle shadows to maintain a flat, professional appearance.

- **Level 0 (Background):** #F7F9FB.
- **Level 1 (Cards/Surface):** White (#FFFFFF) with a 1px solid #E3E8EF border.
- **Shadows:** Use a single, soft ambient shadow for cards: `0px 2px 4px rgba(26, 82, 118, 0.05)`. 
- **Active State:** Elements may gain a slightly more pronounced shadow on hover to indicate interactivity, but should never appear "floating" far above the surface.

## Shapes
The shape language is conservative and geometric. A **Soft (0.25rem)** border radius is applied to buttons, input fields, and cards. This slight rounding softens the "brutality" of a data-dense interface while remaining sharp enough to feel professional and technical.

## Components

### Cards & Access Status
Cards are the primary container for talent profiles and data insights.
- **Visual Rule:** Every card must feature a 4px solid left border. The color of this border must strictly correspond to the `Access Status Palette`.
- **Icons:** Use the designated padlock icons (Closed/Open) next to the status label within the card header.

### Sidebar
The sidebar uses the Primary Navy (#1A5276) background.
- **Icons/Labels:** Use Light Blue (#AED6F1) for inactive states and White (#FFFFFF) for active states.
- **Active Indicator:** A vertical 4px bar on the far left of the menu item in Accent Blue.

### Buttons
- **Primary:** Filled Accent Blue (#2E86C1) with white text.
- **Secondary:** Ghost style with #E3E8EF border and Primary Navy text.
- **Tertiary/Status:** Small buttons using the status colors for specific data actions (e.g., "Upgrade Access").

### Input Fields
Standardized height of 40px. Use #FFFFFF background, #E3E8EF border, and 14px Inter text. Focus states should use a 1px Accent Blue border with a soft blue glow.

### Tables
Crucial for data density. Rows should be 48px tall with 1px bottom borders. Alternating row colors are not required; use hover highlights in #F0F4F8 to assist line tracking.