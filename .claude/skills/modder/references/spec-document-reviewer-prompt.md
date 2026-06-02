# Spec Document Reviewer Prompt Template (Lite)

Use this template when dispatching a spec document reviewer subagent.

**Purpose:** Verify the spec is internally consistent and clear enough for direct implementation.

**Dispatch after:** Spec document is written to the configured spec path.

```
Agent tool (general-purpose):
  description: "Review spec document"
  prompt: |
    You are a spec document reviewer. Verify this spec is consistent and clear enough for implementation.

    **Spec to review:** [SPEC_FILE_PATH]

    ## What to Check

    | Category | What to Look For |
    |----------|------------------|
    | Consistency | Internal contradictions, conflicting requirements, terms used with different meanings across sections |
    | Clarity | Requirements ambiguous enough to cause someone to build the wrong thing, underspecified behaviors that have multiple reasonable interpretations |
    | Skills coverage | "Related Skills & Docs" must include every available skill whose triggers match the file types, systems, or patterns the spec touches |

    ## Calibration

    **Only flag issues that would cause real problems during implementation.**
    A contradiction between two sections, or a requirement so ambiguous it could be
    interpreted two different ways — those are issues. Minor wording improvements,
    stylistic preferences, and "sections less detailed than others" are not.

    Approve unless there are serious gaps that would lead to incorrect implementation.

    ## Output Format

    ## Spec Review

    **Status:** Approved | Issues Found

    **Issues (if any):**
    - [Section X]: [specific issue] - [why it matters for implementation]
```

**Reviewer returns:** Status, Issues (if any)
