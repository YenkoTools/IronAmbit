# Release Please Setup Guide

This repository uses [Release Please](https://github.com/googleapis/release-please) to automate releases for both the Client (AstroJs) and Service (.NET) projects.

## Overview

Release Please automates:
- CHANGELOG generation
- Version bumping
- GitHub Release creation
- Tag creation

It works by analyzing your git commit history, looking for [Conventional Commit](https://www.conventionalcommits.org/) messages, and creating Release PRs.

## Configuration

### Files Created

1. **`release-please-config.json`** - Main configuration file
   - Configures Client with `node` release type
   - Configures Service with `simple` release type
   - Both projects use separate pull requests for independent releases

2. **`.release-please-manifest.json`** - Version tracking file
   - Tracks the current version of each project
   - Updated automatically by Release Please

3. **`Service/version.txt`** - Version file for Service project
   - Required by the `simple` release type
   - Updated automatically on each release

4. **`.github/workflows/release-please.yml`** - GitHub Actions workflow
   - Triggers on push to `main` and `release/**` branches
   - Creates/updates Release PRs
   - Creates GitHub Releases when Release PRs are merged

## How It Works

### 1. Write Commits Using Conventional Commits

Release Please requires commits to follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```bash
# Patch release (0.1.0 -> 0.1.1)
git commit -m "fix: resolve login bug"
git commit -m "fix(api): handle null response"

# Minor release (0.1.0 -> 0.2.0)
git commit -m "feat: add user profile page"
git commit -m "feat(client): implement dark mode"

# Major release (0.1.0 -> 1.0.0)
git commit -m "feat!: redesign authentication flow"
# OR
git commit -m "feat: redesign authentication flow

BREAKING CHANGE: old auth tokens are no longer valid"
```

**Common prefixes:**
- `feat:` - New feature (minor version bump)
- `fix:` - Bug fix (patch version bump)
- `docs:` - Documentation changes (no version bump)
- `chore:` - Maintenance tasks (no version bump)
- `refactor:` - Code refactoring (no version bump)
- `test:` - Test updates (no version bump)
- `!` - Breaking change (major version bump)

**Scopes** (optional):
- Helps organize commits: `feat(client):`, `fix(service):`, `feat(exercises):`

### 2. Push to a Release Branch

When you push commits to `main` or a `release/**` branch:

```bash
git push origin main
# OR
git push origin release/v1.0
```

The GitHub Action will:
1. Analyze commits since the last release
2. Create or update a Release PR for each project that has changes
3. Generate/update CHANGELOG.md for each project

### 3. Review and Merge Release PR

Release Please will create PRs like:
- `chore(main): release Client 1.2.0`
- `chore(main): release Service 0.5.0`

Each PR includes:
- Updated version numbers
- Generated CHANGELOG entries
- Updated package files

**Review the PR** and when ready, **merge it**.

### 4. Automatic Release Creation

When you merge a Release PR:
1. Release Please creates a GitHub Release
2. Tags are created (e.g., `Client-v1.2.0`, `Service-v0.5.0`)
3. Optional deployment steps run (if configured)

## Project-Specific Configuration

### Client (AstroJs Application)

- **Release Type**: `node`
- **Version Location**: `Client/package.json`
- **CHANGELOG**: `Client/CHANGELOG.md`
- **Tags**: `Client-v{version}` (e.g., `Client-v1.2.3`)
- **Extra Files Updated**: `Client/public/version.json`

### Service (.NET Application)

- **Release Type**: `simple`
- **Version Location**: `Service/version.txt`
- **CHANGELOG**: `Service/CHANGELOG.md`
- **Tags**: `Service-v{version}` (e.g., `Service-v0.5.0`)

## Customization Options

### Trigger Branches

The workflow currently triggers on:
- `main` - Primary development branch
- `release/**` - Any branch matching this pattern

To add more branches, edit `.github/workflows/release-please.yml`:

```yaml
on:
  push:
    branches:
      - main
      - develop
      - 'release/**'
      - 'hotfix/**'
```

### Separate vs Combined PRs

Currently configured for **separate pull requests** (one per project).

To create a single combined PR, edit `release-please-config.json`:

```json
{
  "separate-pull-requests": false,
  ...
}
```

### Pre-releases

To mark releases as pre-releases, edit configuration:

```json
{
  "packages": {
    "Client": {
      "prerelease": true,
      "prerelease-type": "beta"
    }
  }
}
```

### Draft Releases

To create draft releases (requires manual publishing):

```json
{
  "packages": {
    "Client": {
      "draft": true
    }
  }
}
```

## GitHub Token Configuration

### Default Behavior (Current Setup)

The workflow uses `GITHUB_TOKEN` which is automatically provided by GitHub Actions.

**Limitation**: Releases and PRs created by `GITHUB_TOKEN` will **not** trigger other workflow runs.

### Using a Personal Access Token (PAT)

If you need release PRs to trigger other workflows (CI checks, deployments, etc.):

1. Create a [Personal Access Token](https://github.com/settings/tokens/new) with `repo` scope
2. Add it as a repository secret (e.g., `RELEASE_PLEASE_TOKEN`)
3. Update the workflow:

```yaml
- uses: googleapis/release-please-action@v4
  with:
    token: ${{ secrets.RELEASE_PLEASE_TOKEN }}
```

## Manual Release Version Override

To manually set a release version, include in your commit:

```bash
git commit --allow-empty -m "chore: release 2.0.0" -m "Release-As: 2.0.0"
```

## Workflow Outputs

The action provides outputs you can use in subsequent steps:

- `releases_created` - Whether any releases were created
- `paths_released` - JSON array of paths released
- `Client--release_created` - Whether Client was released
- `Client--version` - Version number for Client
- `Client--tag_name` - Git tag for Client release
- `Service--release_created` - Whether Service was released
- `Service--version` - Version number for Service
- `Service--tag_name` - Git tag for Service release

Example usage in the workflow:

```yaml
- name: Deploy Client
  if: ${{ steps.release.outputs['Client--release_created'] }}
  run: |
    echo "Deploying Client version ${{ steps.release.outputs['Client--version'] }}"
    # Add deployment commands here
```

## Common Scenarios

### Scenario 1: Bug Fix to Client Only

```bash
cd Client
# Make your changes
git add .
git commit -m "fix(client): resolve navigation issue"
git push origin main
```

Result: Release PR created for Client only

### Scenario 2: New Feature in Service Only

```bash
cd Service
# Make your changes
git add .
git commit -m "feat(service): add workout tracking API"
git push origin main
```

Result: Release PR created for Service only

### Scenario 3: Changes in Both Projects

```bash
# Changes to both projects
git add Client/ Service/
git commit -m "feat: add user preferences sync"
git push origin main
```

Result: Two separate Release PRs created (one for Client, one for Service)

### Scenario 4: Release from a Feature Branch

```bash
git checkout -b release/v2.0
# Make changes
git commit -m "feat!: major UI redesign"
git push origin release/v2.0
```

Result: Release PR created based on `release/v2.0` branch

## Troubleshooting

### Commit Parse Warnings

You may see warnings like:
```
❯ commit could not be parsed: 7e466ea Merge branch 'develop'
❯ error message: Error: unexpected token ' ' at 1:6
```

**These are warnings, not errors.** Release Please will:
- Skip commits it can't parse
- Continue processing valid conventional commits
- Still generate releases based on parseable commits

**Common unparseable commits:**
- Merge commits: `Merge pull request #12`
- Legacy commits: `Refactor code structure...`
- Non-conventional commits: `sanity`, `Update README`

**Solution:** These warnings are normal for repositories with legacy commits. Going forward:
1. Use conventional commit format for all new commits
2. Configure merge commit messages to use conventional format
3. The warnings won't prevent releases from being created

### No Release PR Created

1. **Check commits** - Ensure you're using conventional commit format
2. **Verify branch** - Make sure you pushed to a configured branch
3. **Check labels** - Ensure no old `autorelease: pending` labels exist
4. **Review logs** - Check the GitHub Action logs for errors

### Multiple Release PRs for Same Version

This happens when a Release PR wasn't merged. Close/merge the old PR, and remove the `autorelease: pending` label if it exists.

### Want to Skip a Release

Add `Release-As: 0.1.0` (current version) to prevent version bump, or use:

```bash
git commit -m "chore: update documentation"
```

Commits with `chore:`, `docs:`, `test:` don't trigger releases.

## Additional Resources

- [Release Please Documentation](https://github.com/googleapis/release-please)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Release Please Action](https://github.com/googleapis/release-please-action)
- [Manifest Configuration](https://github.com/googleapis/release-please/blob/main/docs/manifest-releaser.md)

## Next Steps

1. **Test the setup**: Create a test commit with a conventional commit message
2. **Review the Release PR**: Check that Release Please creates appropriate PRs
3. **Customize deployment**: Add deployment steps to the workflow as needed
4. **Update team documentation**: Ensure everyone knows to use conventional commits
