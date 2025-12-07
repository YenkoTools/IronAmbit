# Version Management Strategy

**Project**: IronAmbit  
**Date**: December 7, 2025  
**Topic**: Synchronizing version numbers across git tags, package.json, and version.json files

---

## Current Setup

### Existing Infrastructure
- **Git Tags**: Source of truth for version numbers (e.g., `v0.2.0`)
- **PowerShell Script**: `version.ps1` generates `version.json` files with build metadata
- **package.json**: Currently manually maintained (v0.0.3)
- **Output Files**:
  - `Client/public/version.json` - Runtime version information
  - `Service/src/Api/wwwroot/version.json` - Runtime version information

### The Challenge
Currently, there's a disconnect between git tags (v0.2.0) and `package.json` version (0.0.3). We need automation to keep these in sync.

---

## Standard Tools for Version Management

### 1. **`npm version`** (Built-in)
**Best for**: Simple version bumps

```bash
npm version patch  # 0.0.3 -> 0.0.4
npm version minor  # 0.0.3 -> 0.1.0
npm version major  # 0.0.3 -> 1.0.0
```

**Pros**:
- No additional dependencies
- Simple and straightforward
- Automatically creates git commit and tag

**Cons**:
- Limited customization
- Basic functionality only

---

### 2. **`release-it`** ⭐ **RECOMMENDED**
**Best for**: Automated releases with changelog generation

**Features**:
- Updates `package.json` version
- Creates git tags
- Generates changelogs
- Runs custom hooks/scripts
- Can trigger existing `version.ps1` script

**Installation**:
```bash
cd Client
npm install -D release-it
```

**Configuration** (`.release-it.json`):
```json
{
  "git": {
    "tagName": "v${version}",
    "commitMessage": "chore: release v${version}",
    "requireCleanWorkingDir": false
  },
  "npm": {
    "publish": false
  },
  "hooks": {
    "after:bump": "pwsh ../../version.ps1"
  }
}
```

**Usage**:
```bash
npm run release -- patch
npm run release -- minor
npm run release -- major
```

---

### 3. **`standard-version`** / **`commit-and-tag-version`**
**Best for**: Conventional commit-based versioning

**Features**:
- Automates `CHANGELOG.md` based on git commits
- Updates package.json and creates tags
- Works well with existing `CHANGELOG.md` format
- Follows semantic versioning

**Installation**:
```bash
npm install -D commit-and-tag-version
```

**Usage**:
```bash
npm run release
```

---

### 4. **`semantic-release`**
**Best for**: Fully automated semantic versioning

**Features**:
- CI/CD focused
- Analyzes commits to determine version bump
- Most comprehensive solution
- Automated releases based on commit messages

**Pros**:
- Fully automated
- Great for CI/CD pipelines
- Comprehensive plugin ecosystem

**Cons**:
- Complex setup
- Requires conventional commit format
- Steeper learning curve

---

### 5. **`bumpp`**
**Best for**: Simple, modern alternative

**Features**:
- Updates multiple package.json files (monorepo support)
- Creates git tags
- Lightweight and fast
- Modern TypeScript implementation

**Installation**:
```bash
npm install -D bumpp
```

**Usage**:
```bash
npx bumpp
```

---

## Enhanced version.ps1 Script

Below is an enhanced version of the existing `version.ps1` script that also updates `package.json`:

```powershell
#!/usr/bin/env pwsh
# Requires PowerShell 7+

# Exit on error
$ErrorActionPreference = "Stop"

# ------------------------
# Update version.json and package.json
# ------------------------

# Get script directory
$SCRIPT_DIR = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

# Fetch the latest tags with more git history
git fetch --prune --unshallow --tags 2>$null

# Get the most recent tag with fallback options
$GIT_TAG = $null
$GIT_TAG = git describe --tags --abbrev=0 2>$null
if (-not $GIT_TAG) {
    # Try getting tag from GitHub Actions environment variable
    $GIT_TAG = $env:GITHUB_REF
    if ($GIT_TAG -and $GIT_TAG.StartsWith("refs/tags/")) {
        $GIT_TAG = $GIT_TAG.Substring(10)
    } else {
        $GIT_TAG = "v0.0.0"
    }
}

# Extract version without 'v' prefix for package.json
$VERSION_NUMBER = $GIT_TAG -replace '^v', ''

# Get short commit hash
$COMMIT_HASH = git rev-parse --short HEAD

# Build date info
$NOW = Get-Date -UFormat %s
$MIDNIGHT = (Get-Date -Date (Get-Date).Date -UFormat %s)
$MINUTES_SINCE_MIDNIGHT = [math]::Floor(($NOW - $MIDNIGHT) / 60)
$BUILD_TICK = $MINUTES_SINCE_MIDNIGHT

# Format build date
$BUILD_DATE = Get-Date -Format "yyyy-MM-ddTHH:mm:ssK"

# Build number (YYYYMMDD.Tick)
$DATE = Get-Date -Format "yyyyMMdd"
$BUILD_NUMBER = "$DATE.$BUILD_TICK"

# Get branch and full commit hash
$GIT_BRANCH = git rev-parse --abbrev-ref HEAD
$GIT_HEAD = git rev-parse HEAD

# Print build number
Write-Host "Build Number: $BUILD_NUMBER"
Write-Host "Version: $VERSION_NUMBER"

# Create JSON object for version.json
$JSON = @{
    BuildNumber = $BUILD_NUMBER
    BuildDate   = $BUILD_DATE
    CommitHash  = $COMMIT_HASH
    GitBranch   = $GIT_BRANCH
    GitHead     = $GIT_HEAD
    GitTag      = $GIT_TAG
} | ConvertTo-Json -Depth 10 -Compress

# Write to version.json files
$OUTPUT_PATH = Join-Path -Path $SCRIPT_DIR -ChildPath "Client/public/version.json"
New-Item -ItemType Directory -Path (Split-Path -Parent $OUTPUT_PATH) -Force | Out-Null
$JSON | Set-Content -Path $OUTPUT_PATH -Encoding UTF8
Write-Host "✅ version.json written to $OUTPUT_PATH"

$OUTPUT_PATH = Join-Path -Path $SCRIPT_DIR -ChildPath "Service/src/Api/wwwroot/version.json"
New-Item -ItemType Directory -Path (Split-Path -Parent $OUTPUT_PATH) -Force | Out-Null
$JSON | Set-Content -Path $OUTPUT_PATH -Encoding UTF8
Write-Host "✅ version.json written to $OUTPUT_PATH"

# ------------------------
# Update package.json version
# ------------------------
$PACKAGE_JSON_PATH = Join-Path -Path $SCRIPT_DIR -ChildPath "Client/package.json"

if (Test-Path $PACKAGE_JSON_PATH) {
    # Read package.json
    $packageContent = Get-Content -Path $PACKAGE_JSON_PATH -Raw | ConvertFrom-Json
    
    # Update version
    $oldVersion = $packageContent.version
    $packageContent.version = $VERSION_NUMBER
    
    # Write back to package.json with proper formatting
    $packageContent | ConvertTo-Json -Depth 100 | Set-Content -Path $PACKAGE_JSON_PATH -Encoding UTF8
    
    Write-Host "✅ Updated package.json version from $oldVersion to $VERSION_NUMBER"
} else {
    Write-Warning "⚠️  package.json not found at $PACKAGE_JSON_PATH"
}
```

### Alternative: Using Node.js for Better JSON Formatting

Add this to the end of your existing `version.ps1`:

```powershell
# Update package.json using Node.js for proper formatting
$nodeScript = @"
const fs = require('fs');
const path = require('path');
const pkgPath = path.join(__dirname, 'Client', 'package.json');
const pkg = JSON.parse(fs.readFileSync(pkgPath, 'utf8'));
pkg.version = '$VERSION_NUMBER';
fs.writeFileSync(pkgPath, JSON.stringify(pkg, null, 2) + '\n', 'utf8');
console.log('✅ Updated package.json to version $VERSION_NUMBER');
"@

node -e $nodeScript
```

---

## Best Practices & Industry Standards

### Single Source of Truth

Most teams choose one of these approaches:

1. **Git tags as source** → Update package.json from tags
   - Good for projects where releases are manually controlled
   - Your current approach
   
2. **package.json as source** → Create git tags from package.json
   - More common in Node.js ecosystem
   - Preferred by npm-based tools

### Automated Version Management

**Best Practices**:
- Use tools like `release-it`, `semantic-release`, or `standard-version`
- Integrate with CI/CD pipelines
- **Never manually edit version numbers**
- Automate changelog generation
- Enforce conventional commits for clarity

### Monorepo Considerations

For projects with multiple packages (like IronAmbit with Client and Service):
- Consider tools like `changesets`, `lerna`, or `nx`
- Maintain consistent versioning across packages
- Use workspace-aware version management

### Build-Time Version Injection

**Current Approach is Good**:
- `Client/public/version.json` for runtime information ✅
- `package.json` version for package management ✅
- Both should stay in sync but serve different purposes

### Conventional Commits

If adopting automated versioning, use conventional commits:
```
feat: add new feature (minor bump)
fix: bug fix (patch bump)
BREAKING CHANGE: breaking API change (major bump)
chore: maintenance task (no bump)
docs: documentation update (no bump)
```

---

## Assessment of Current Approach

### ✅ **What's Working Well**

1. **Git tags as authoritative version source**
   - Clear, immutable version history
   - Standard git practice
   
2. **Separate version.json for build metadata**
   - Provides detailed build information
   - Useful for debugging and tracking
   
3. **PowerShell script automation**
   - Cross-platform (PowerShell 7+)
   - Flexible and customizable

### ⚠️ **Areas for Improvement**

1. **package.json version is currently manual**
   - Out of sync: git tag is v0.2.0, package.json is 0.0.3
   - Requires manual updates
   
2. **No automation between git tag and package.json update**
   - Risk of human error
   - Extra manual step in release process

---

## Recommended Solution

### **Option A: Enhance version.ps1** (Quick Fix)

**Pros**:
- Minimal changes to existing workflow
- No new dependencies
- Keeps current process intact

**Cons**:
- JSON formatting might not be perfect with PowerShell
- Still requires manual git tagging

**Implementation**: Use the enhanced `version.ps1` script shown above.

---

### **Option B: Use release-it** ⭐ **RECOMMENDED**

**Pros**:
- Industry-standard tool
- Automates entire release process
- Can still use your existing `version.ps1` via hooks
- Better JSON formatting
- Changelog generation
- Interactive prompts

**Cons**:
- Additional dependency
- Learning curve for team

**Implementation**:

1. **Install release-it**:
```bash
cd Client
npm install -D release-it
```

2. **Create `.release-it.json`** in Client directory:
```json
{
  "git": {
    "tagName": "v${version}",
    "commitMessage": "chore: release v${version}",
    "requireCleanWorkingDir": false
  },
  "npm": {
    "publish": false
  },
  "hooks": {
    "after:bump": "pwsh ../../version.ps1"
  },
  "github": {
    "release": true
  },
  "plugins": {
    "@release-it/conventional-changelog": {
      "preset": "angular",
      "infile": "../../CHANGELOG.md"
    }
  }
}
```

3. **Add to package.json scripts**:
```json
{
  "scripts": {
    "release": "release-it",
    "release:patch": "release-it patch",
    "release:minor": "release-it minor",
    "release:major": "release-it major"
  }
}
```

4. **Usage**:
```bash
# Interactive mode (asks which version bump)
npm run release

# Direct version bump
npm run release:patch  # 0.0.3 -> 0.0.4
npm run release:minor  # 0.0.3 -> 0.1.0
npm run release:major  # 0.0.3 -> 1.0.0
```

**What happens automatically**:
1. Prompts for version bump type
2. Updates `package.json` version
3. Runs your `version.ps1` script (updates version.json files)
4. Creates git commit: "chore: release vX.Y.Z"
5. Creates git tag: "vX.Y.Z"
6. Pushes to remote
7. (Optional) Creates GitHub release

---

### **Option C: Use npm version + Hook** (Simplest)

**Pros**:
- No additional dependencies
- Built into npm
- Simple workflow

**Cons**:
- Less flexible
- No changelog automation

**Implementation**:

Add to `Client/package.json`:
```json
{
  "scripts": {
    "version": "pwsh ../../version.ps1",
    "postversion": "git push --follow-tags"
  }
}
```

**Usage**:
```bash
cd Client
npm version patch  # or minor, major
```

This will:
1. Update package.json
2. Run version.ps1 (via "version" script)
3. Create git commit
4. Create git tag
5. Push to remote (via "postversion" script)

---

## Workflow Comparison

### Current Workflow
```
1. Manually update git tag
2. Run version.ps1 → generates version.json
3. Manually update package.json (often forgotten!)
```

### Recommended Workflow (with release-it)
```
1. npm run release
   ├─ Updates package.json
   ├─ Runs version.ps1 → generates version.json
   ├─ Creates git commit
   ├─ Creates git tag
   └─ Pushes to remote
```

### Alternative Workflow (npm version + hook)
```
1. npm version patch/minor/major
   ├─ Updates package.json
   ├─ Runs version.ps1 → generates version.json
   ├─ Creates git commit
   ├─ Creates git tag
   └─ Pushes to remote
```

---

## Implementation Roadmap

### Phase 1: Quick Fix (1 hour)
- [ ] Enhance `version.ps1` to update package.json
- [ ] Test with current workflow
- [ ] Document process

### Phase 2: Tool Evaluation (1-2 hours)
- [ ] Install and test `release-it`
- [ ] Configure for IronAmbit project
- [ ] Test release workflow
- [ ] Train team members

### Phase 3: Adoption (Ongoing)
- [ ] Update documentation
- [ ] Update CI/CD pipelines if needed
- [ ] Establish conventional commit guidelines
- [ ] Monitor and refine process

---

## Conclusion

Your current approach is solid and on the right track. The main gap is the manual synchronization between git tags and `package.json`. 

**Our recommendation**: Implement **Option B (release-it)** for the best balance of automation, flexibility, and industry best practices. This will:
- Eliminate manual steps
- Reduce human error
- Provide better tooling
- Maintain your existing `version.ps1` script for build metadata

Start with the enhanced `version.ps1` script (Option A) if you want a quick fix, then migrate to `release-it` when ready for a more robust solution.

---

## Additional Resources

- [release-it Documentation](https://github.com/release-it/release-it)
- [npm version Documentation](https://docs.npmjs.com/cli/v8/commands/npm-version)
- [Semantic Versioning Specification](https://semver.org/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [commit-and-tag-version](https://github.com/absolute-version/commit-and-tag-version)
- [semantic-release](https://github.com/semantic-release/semantic-release)

---

**Questions or need help implementing?** Feel free to reach out to the team!
