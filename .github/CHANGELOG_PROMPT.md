# Changelog Update Prompt

Use this prompt to update the CHANGELOG.md file based on git commits between tags.

---

## Prompt

```
Please update the CHANGELOG.md file with changes since the last release.

Instructions:
1. Use `git tag --sort=-creatordate` to find the latest tag
2. Use `git log <latest-tag>..HEAD --oneline` to get commits since the last tag
3. Analyze the commits and categorize them into:
   - Added (new features)
   - Changed (changes in existing functionality)
   - Deprecated (soon-to-be removed features)
   - Removed (removed features)
   - Fixed (bug fixes)
   - Security (security fixes)
4. Add a new version section at the top of CHANGELOG.md with:
   - Version number (increment based on changes: major.minor.patch)
   - Release date (today's date in YYYY-MM-DD format)
   - Categorized changes with bullet points
5. Keep existing changelog entries intact
6. Follow Keep a Changelog format (https://keepachangelog.com/)
7. Write clear, user-focused descriptions (not technical commit messages)
8. Group related commits into single changelog entries when appropriate

If there are no tags yet, analyze all commits from the beginning of the repository.
```

---

## Example Usage

**User says:**
> Update the changelog for version 0.2.0

**Or simply:**
> Update the changelog

**Or for a specific tag range:**
> Update the changelog with changes between v0.1.0 and HEAD

---

## Expected Output Format

The tool should update CHANGELOG.md to look like this:

```markdown
# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2025-12-02

### Added
- User management page with pagination
- Exercise library management page
- Workout tracking page with daily logs
- Mock API service for development
- Comprehensive API integration documentation

### Changed
- Updated Header component with navigation links
- Improved error handling in DataTable component

### Fixed
- TypeScript errors in table components
- Pagination calculation for edge cases

## [0.1.0] - 2025-11-15

### Added
- Initial project setup with Astro and React
- Basic layout and styling with Tailwind CSS
- Home and About pages

[Unreleased]: https://github.com/YenkoTools/IronAmbit/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/YenkoTools/IronAmbit/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/YenkoTools/IronAmbit/releases/tag/v0.1.0
```

---

## Notes

- If no version number is specified, the tool should suggest the next version based on the types of changes
- Major version (X.0.0): Breaking changes or major new features
- Minor version (0.X.0): New features, backward compatible
- Patch version (0.0.X): Bug fixes and minor improvements
- The tool will automatically determine the appropriate version increment
