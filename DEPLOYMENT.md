# LaunchCart Deployment Guide

## Overview

LaunchCart uses automated GitHub Actions workflows to deploy approved and merged code to the Azure environment. When code is merged to the `main` branch, a GitHub Actions workflow automatically validates all status checks (tests, linting, security scans) and deploys to Azure App Service if checks pass. This guide covers the merge strategy, approval requirements, deployment process, and rollback procedures.

## Merge Strategy to `main`

### Process

1. **Create a feature branch** from `main`:
   ```bash
   git checkout -b feature/my-feature
   ```

2. **Develop and commit** your changes on the feature branch:
   ```bash
   git add .
   git commit -m "feat: add my feature"
   ```

3. **Push to remote** and open a Pull Request:
   ```bash
   git push origin feature/my-feature
   ```

4. **Request code review** by assigning reviewers (see "Approval & Review Requirements" below)

5. **Address review feedback** and push additional commits:
   ```bash
   git commit -m "fix: address review comments"
   git push origin feature/my-feature
   ```

6. **All status checks must pass** before merge is allowed:
   - CI/CD pipeline (build, tests, linting)
   - Security scans
   - Branch protection rule checks

7. **Merge to `main`** manually via GitHub UI (do not auto-merge):
   - Use **Squash and merge** (recommended) to keep history clean, or
   - Use **Rebase and merge** to preserve individual commits
   - **Do NOT** use default merge commit strategy unless specifically required
   - After merge, delete the feature branch

### Approved Merge Strategies

- **Squash and merge** (RECOMMENDED): Combines all commits into a single commit with a clear message. Keeps `main` history concise.
- **Rebase and merge**: Replays commits on top of `main`. Useful if commit history is important for traceability.
- **Create a merge commit**: Preserves full branch history. Use only if required by project standards.

**Avoid:** Force pushes to `main`, rebasing `main`, or circumventing branch protection rules.

## Approval & Review Requirements

### Branch Protection Rules

The following rules are enforced on the `main` branch:

1. **Require pull request reviews before merging**
   - Minimum 1 approval required from code owner or senior engineer
   - Review must be recent (within 2 weeks of approval)
   - Dismissing stale approvals is enabled

2. **Require status checks to pass before merging**
   - All CI/CD checks must pass:
     - `build` — ASP.NET Core build and .NET tests
     - `test` — Unit and integration tests
     - `lint` — Code style and formatting
     - `security-scan` — SAST and dependency checks
   - All checks must be passing on the latest commit before merge is allowed

3. **Require conversation resolution**
   - All discussion threads must be marked as "Resolved" before merge

4. **Require branches to be up to date before merging**
   - Feature branch must be synced with `main` before merge is allowed
   - Automatic sync: Pull latest from `main` locally and push: `git rebase main && git push -f`

### Code Review Checklist

When reviewing a PR, verify:

- [ ] Code follows project style and conventions (check linting results)
- [ ] Tests are included and passing for new functionality
- [ ] Security considerations: no hardcoded secrets, SQL injection risks, XSS vulnerabilities, etc.
- [ ] Database migrations (if applicable) are reversible and tested
- [ ] Documentation is updated (README, DEPLOYMENT.md, API docs, etc.)
- [ ] Performance implications are considered (no N+1 queries, large loops, etc.)
- [ ] Error handling is appropriate and user-facing messages are clear
- [ ] No breaking changes to public APIs without deprecation notice

## Deployment Process

### Automatic Deployment Workflow

When code is merged to `main`, the `Deploy to Azure` GitHub Actions workflow automatically triggers:

1. **Checkout code** from `main`
2. **Set up .NET 6.0 runtime**
3. **Restore NuGet dependencies** (`dotnet restore`)
4. **Build ASP.NET Core API** in Release mode (`dotnet build`)
5. **Publish to artifact directory** (`dotnet publish`)
6. **Authenticate to Azure** using service principal credentials from GitHub Secrets
7. **Deploy to Azure App Service** using Azure CLI (`az webapp up`)
8. **Log completion** with timestamp

**Workflow location:** `.github/workflows/deploy-to-azure.yml`  
**Logs:** https://github.com/kcsnap/launchcart/actions (filter by "Deploy to Azure" workflow)

### Deployment Configuration

- **Azure App Service Name:** Stored in `AZURE_APP_SERVICE_NAME` GitHub Secret
- **Resource Group:** Stored in `AZURE_RESOURCE_GROUP` GitHub Secret
- **Subscription ID:** Stored in `AZURE_SUBSCRIPTION_ID` GitHub Secret
- **Service Principal Credentials:** Stored in `AZURE_CREDENTIALS` GitHub Secret (JSON format with `clientId`, `clientSecret`, `tenantId`)
- **Runtime:** .NET 6.0 on Azure App Service (Linux or Windows)
- **Region:** Check Azure Portal for App Service location

### Deployment Environment

- **Target Environment:** Azure App Service (production)
- **URL:** `https://<app-service-name>.azurewebsites.net`
- **API Endpoint:** `https://<app-service-name>.azurewebsites.net/api`

## Deployment Frequency & SLA

### Expected Frequency

- **Typical:** 5–10 deployments per day
- **Peak:** 15+ deployments per day during feature sprints
- **Minimum:** At least one deployment per sprint (every 2 weeks)

### Deployment SLA

- **Target time-to-deployment:** 10 minutes from merge to live
- **Expected uptime:** 99.5% (Azure SLA, not guaranteed)
- **Deployment windows:** Continuous (no blackout periods or maintenance windows)

### Monitoring Deployment Status

**GitHub Actions:**
1. Navigate to https://github.com/kcsnap/launchcart/actions
2. Click on the "Deploy to Azure" workflow
3. View the latest run; green checkmark = success, red X = failed
4. Click into a run to view detailed logs for each step

**Azure Portal:**
1. Go to Azure Portal → search for LaunchCart App Service
2. Click on **Deployment Center** to view deployment history
3. Click on **Logs** or **Monitoring** to view runtime errors and performance

## Monitoring & Logs

### GitHub Actions Logs

Access deployment workflow logs:
- **URL:** https://github.com/kcsnap/launchcart/actions/workflows/deploy-to-azure.yml
- **View logs:** Click a workflow run → scroll to view step-by-step output
- **Search logs:** Use GitHub UI filter or download run logs as artifact
- **Retention:** Logs retained for 90 days (GitHub default)

**Log contents:**
- .NET build output (warnings, errors, assembly info)
- Test results and coverage
- Azure authentication status
- Deployment command output and status
- Timestamps and duration

### Azure App Service Logs

Access application runtime logs:
- **Azure Portal:** App Service → **Logs** → **Log Stream**
- **Application Insights:** App Service → **Application Insights** → view metrics and traces
- **Activity Log:** View deployment and configuration changes

### Secrets Masking

All GitHub Secrets (Azure credentials) are automatically masked in logs:
- `***` appears instead of actual secret values
- Prevents accidental credential exposure
- If a secret is logged, contact security team immediately

## Rollback Procedure

### Rollback via Git Revert (Recommended)

1. **Identify the problematic commit:**
   - Go to https://github.com/kcsnap/launchcart/commits/main
   - Find the commit that introduced the issue (check timestamp and commit message)
   - Copy the full commit SHA (e.g., `abc123def456`)

2. **Create a revert commit locally:**
   ```bash
   git fetch origin main
   git checkout main
   git pull origin main
   git revert abc123def456
   ```

3. **Push the revert to `main`:**
   ```bash
   git push origin main
   ```

4. **Monitor the deployment:**
   - GitHub Actions workflow automatically triggers
   - Check https://github.com/kcsnap/launchcart/actions for status
   - Deployment should complete within 10 minutes
   - Verify live site is restored to previous version

**Advantages:** Clean audit trail, fully automated re-deployment, easy to track what was reverted

### Rollback via Azure App Service Deployment Slots (Alternative)

If Azure App Service deployment slots are configured:

1. **Identify the previous successful deployment** in Azure Portal
2. **Navigate to Deployment Slots:** App Service → **Deployment slots**
3. **Swap slots:** Right-click "staging" slot → **Swap with production**
4. **Verify:** Check live site immediately (no rebuild required, instant swap)

**Advantages:** Instant rollback (seconds, not minutes), no git history pollution

**Disadvantages:** Requires pre-configured slots; not available by default

### Manual Rollback (Emergency Only)

If git revert and slot swap are not available:

1. **Restore a previous deployment artifact** from backup or previous build
2. **Manually publish** to Azure App Service using Azure CLI:
   ```bash
   az webapp deployment source config-zip \
     --resource-group $AZURE_RESOURCE_GROUP \
     --name $AZURE_APP_SERVICE_NAME \
     --src ./previous-build.zip
   ```
3. **Verify:** Check live site and logs for errors

**Contact DevOps team** if you need to execute manual rollback.

## Troubleshooting Failed Deployments

### Common Issues & Solutions

#### Issue: GitHub Actions Workflow Failed at "Build" Step

**Error message in logs:** `Build FAILED`, `error CS*`, or compilation errors

**Solution:**
1. Review the exact error in GitHub Actions logs
2. Reproduce locally: `dotnet build src/api --configuration Release`
3. Fix the issue on the feature branch
4. Commit and push; wait for new PR checks to pass
5. Merge to `main` only after checks are green

---

#### Issue: Azure Authentication Failed

**Error message in logs:** `ERROR: The subscription of '<subscription-id>' could not be found`, `InvalidAuthenticationTokenTenant`, or `Permission denied`

**Solution:**
1. Verify Azure credentials in GitHub Secrets are correct:
   - `AZURE_CREDENTIALS` contains valid JSON with `clientId`, `clientSecret`, `tenantId`, `subscriptionId`
   - `AZURE_SUBSCRIPTION_ID` matches the subscription ID
2. Verify service principal has permissions:
   - Azure Portal → Subscriptions → **Access control (IAM)**
   - Service principal should have **Contributor** or **App Service Contributor** role
3. Check if credentials have expired:
   - Service principal secrets expire annually; rotate in Azure AD if needed
   - Contact DevOps team to refresh secrets in GitHub
4. Re-run the workflow after secrets are updated

---

#### Issue: Azure App Service Not Found

**Error message in logs:** `(NotFound) Resource group '<rg>' could not be found`, `The App Service Plan named '<plan>' could not be found`

**Solution:**
1. Verify GitHub Secrets match actual Azure resources:
   - `AZURE_RESOURCE_GROUP`: Check in Azure Portal → Resource groups
   - `AZURE_APP_SERVICE_NAME`: Check in Azure Portal → App Services
   - `AZURE_SUBSCRIPTION_ID`: Check in Azure Portal → Subscriptions
2. Ensure secrets are spelled correctly (case-sensitive)
3. Verify service principal has access to the resource group and subscription
4. Contact DevOps team if resources have been renamed or moved

---

#### Issue: Deployment Completed But App is Not Responding

**Symptoms:** Workflow succeeds, but live site is down or returning errors

**Solution:**
1. Check Azure App Service logs:
   - Azure Portal → App Service → **Log Stream**
   - Look for runtime errors, missing dependencies, or connection string issues
2. Verify environment variables are set in Azure:
   - App Service → **Configuration** → **Application settings**
   - Check `ConnectionStrings`, `AppSettings`, etc.
3. Restart the App Service:
   - Azure Portal → App Service → **Overview** → **Restart**
4. Rollback to previous version (see "Rollback Procedure" above)
5. Contact on-call engineer or DevOps team for production support

---

#### Issue: Tests Failing in GitHub Actions (Blocking Merge)

**Error message in logs:** Test names and assertion failures

**Solution:**
1. Reproduce the failing test locally:
   ```bash
   cd src/api
   dotnet test --configuration Release --no-build
   ```
2. Fix the test or the code it's testing
3. Push fixes to feature branch
4. Wait for GitHub Actions to re-run tests
5. Merge to `main` only after all tests pass

---

#### Issue: Linting or Security Scan Failing (Blocking Merge)

**Error message in logs:** Code style violations, security findings, or dependency vulnerabilities

**Solution:**
1. Review the specific violation in GitHub Actions logs
2. Fix the code or configuration locally:
   - Code style: Run `dotnet format` and review changes
   - Security: Address the specific vulnerability (patch dependency, fix code, etc.)
3. Commit and push to feature branch
4. Wait for checks to re-run and pass
5. Merge to `main` once all checks are green

---

### Getting Help

- **Build or test failures:** Contact the developer who introduced the change or code owner
- **Azure or credentials issues:** Contact DevOps team (Slack: #devops or escalate via PagerDuty)
- **Deployment SLA or process questions:** Contact engineering lead
- **Security incidents (leaked credentials):** Contact security team immediately

## Manual Deployment (Emergency Only)

Use manual deployment only if the GitHub Actions workflow is broken or unavailable.

### Prerequisites

- Azure CLI installed locally (`az --version`)
- Valid Azure credentials configured or service principal JSON
- `.NET 6.0 SDK` installed locally

### Steps

1. **Clone the repository and checkout `main`:**
   ```bash
   git clone https://github.com/kcsnap/launchcart.git
   cd launchcart
   git checkout main
   git pull origin main
   ```

2. **Build and publish:**
   ```bash
   cd src/api
   dotnet restore
   dotnet build --configuration Release
   dotnet publish -c Release -o ./publish
   cd ../../
   ```

3. **Authenticate to Azure:**
   ```bash
   az login --service-principal -u $AZURE_CLIENT_ID -p $AZURE_CLIENT_SECRET --tenant $AZURE_TENANT_ID
   az account set --subscription $AZURE_SUBSCRIPTION_ID
   ```

4. **Deploy to App Service:**
   ```bash
   az webapp up \
     --resource-group $AZURE_RESOURCE_GROUP \
     --name $AZURE_APP_SERVICE_NAME \
     --subscription $AZURE_SUBSCRIPTION_ID
   ```

5. **Verify deployment:**
   - Check https://\<app-service-name\>.azurewebsites.net
   - Monitor Azure logs for errors

**Important:** After manual deployment, verify the code state is correct and aligned with `main` branch. Log the manual deployment in your team's incident tracking (Slack, Jira, etc.).

## Security & Secrets

### GitHub Secrets

All Azure credentials are stored securely as GitHub Secrets and **never exposed in logs**:

- `AZURE_CREDENTIALS` — JSON blob with service principal details
- `AZURE_SUBSCRIPTION_ID` — Azure subscription ID
- `AZURE_RESOURCE_GROUP` — Azure resource group name
- `AZURE_APP_SERVICE_NAME` — Azure App Service name
- `AZURE_APP_SERVICE_PLAN` — Azure App Service plan name

### Secret Management

- **Access:** Only DevOps team and repository admins can view/edit secrets
- **Rotation:** Secrets are rotated quarterly or immediately if compromised
- **Audit:** All secret access is logged; check GitHub audit log regularly
- **Never:** Commit secrets to git, log secrets in output, or share secrets via email/Slack

### If Credentials Are Compromised

1. **Stop all deployments immediately** (disable the GitHub Actions workflow if necessary)
2. **Contact security team** and DevOps team
3. **Rotate the service principal** in Azure AD
4. **Update GitHub Secrets** with new credentials
5. **Re-enable deployments** after secrets are updated
6. **Audit all recent deployments** for unauthorized changes

## Post-Deployment Validation

### Manual Checks (Required)

After deployment, verify the live application:

1. **Homepage:** Navigate to https://\<app-service-name\>.azurewebsites.net
   - Page loads without errors
   - Featured products are displayed
   - Navigation links work

2. **Products Page:** Navigate to `/products`
   - All products load
   - Filters and sorting work
   - Search functionality works

3. **Product Detail:** Click on a product
   - Product details display correctly
   - "Request Enquiry" CTA is visible
   - Links to other pages work

4. **API Health:** Curl or request `/api/products` endpoint
   ```bash
   curl -s https://<app-service-name>.azurewebsites.net/api/products | jq .
   ```
   - Response is valid JSON
   - Status code is 200 OK
   - Product data is present

### Automated Checks (Optional Future Enhancement)

- Post-deployment smoke tests (Selenium, Playwright, etc.)
- Health check endpoint (`/health`, `/api/health`)
- Synthetic monitoring (Uptime.com, Checkly, etc.)

**Note:** Automated post-deployment checks are not in scope for the current implementation.

## Rollback Decision Tree

Use this flowchart to decide on the best rollback approach:

```
Is the issue in code (feature bug, regression)?
├─ YES → Use Git Revert (recommended)
│        Push `git revert <commit-sha>` to main
│        Workflow auto-deploys the reverted code
├─ NO (infrastructure/config issue)?
│  ├─ Can you fix it quickly (< 5 min)?
│  │  └─ YES → Deploy a fix commit
│  │  └─ NO → Proceed to next step
│  └─ Are Azure App Service deployment slots available?
│     ├─ YES → Use Slot Swap (instant rollback)
│     └─ NO → Use Git Revert or manual rollback
└─ Emergency (critical production outage)?
   └─ Contact on-call engineer immediately
      Use slot swap or manual rollback while fix is being prepared
```

## Frequently Asked Questions

### Q: Can I deploy to `main` without a pull request?

**A:** No. Branch protection rules require at least one code review and all status checks to pass before a merge to `main` is allowed. Direct commits to `main` are blocked.

### Q: How do I add a new status check (e.g., performance tests)?

**A:** Contact DevOps team. New checks must be added to the CI/CD pipeline and configured in GitHub branch protection rules. Update this document once the new check is live.

### Q: What if I need to deploy a hotfix immediately?

**A:** Create a feature branch from `main`, commit the fix, open a PR, request expedited review, and merge once approved and tests pass. The GitHub Actions workflow will deploy automatically. Target: deploy within 10 minutes of merge.

### Q: Can I automate the code review or approval?

**A:** No. Code review is a manual process and cannot be automated. Branch protection requires human approval before merge.

### Q: What happens if deployment fails after I merge?

**A:** The workflow will fail and notify the team. Check GitHub Actions logs for the error, fix the issue on a new branch, and merge the fix to `main`. The workflow will re-run and deploy once the fix is merged.

### Q: How do I test the deployment workflow without affecting production?

**A:** Create a test feature branch (e.g., `test/deploy-workflow`) with a small change (comment update, README edit). Open a PR, merge it to `main`, and monitor the deployment. Rollback by reverting the commit if needed.

### Q: Who has permission to trigger manual deployments or rollbacks?

**A:** Any team member with write access to the repository can merge to `main` (triggering auto-deployment) or create a revert commit (triggering rollback). Manual Azure CLI deployments should be performed only by DevOps engineers or on-call engineers.

### Q: How often should I rotate Azure credentials?

**A:** Quarterly (every 3 months) as a standard practice. Immediately if compromised or if a team member leaves.

### Q: Can I deploy to multiple Azure environments (dev, staging, production)?

**A:** Current implementation deploys only to production. To add staging or development environments, contact DevOps team to set up separate workflows and App Service instances.

---

## Support & Escalation

### For Questions or Issues

- **Deployment process:** Contact your engineering lead or DevOps team
- **Code review:** Contact code owners (@codeowners in GitHub)
- **Azure infrastructure:** Contact DevOps team (Slack: #devops)
- **Security incidents:** Contact security@company.com immediately
- **Production outage:** Contact on-call engineer (PagerDuty)

### Contact Information

- **DevOps Team:** Slack: #devops, Email: devops@company.com
- **Engineering Lead:** [Name/Contact]
- **On-Call Rotation:** PagerDuty (link: pagerduty.com/incidents)
- **Security Team:** security@company.com

---

**Last Updated:** [Date]  
**Document Owner:** DevOps Team  
**Version:** 1.0