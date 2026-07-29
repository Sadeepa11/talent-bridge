# SOFTWARE REQUIREMENTS SPECIFICATION

## Candidate Access Platform (working title: **TalentBridge**)

| Field | Value |
|---|---|
| **Project** | Candidate Access Platform — TalentBridge |
| **Version** | v1.0 |
| **Date** | 28 July 2026 |
| **Classification** | Confidential |

---

# Section 1: Introduction

## 1.1 Purpose

This document specifies the functional and non-functional requirements for the Candidate Access Platform, a SaaS product operated by eSupport Technology (Pvt) Ltd. It is written for two audiences: the development team, who will use it as the authoritative build specification, and the business stakeholders, who will use it to confirm that the system behaves as the commercial model requires. It covers user roles, functional requirements, data models, API surface, access-control rules, and the data-protection obligations that apply to the processing of candidate personal data.

## 1.2 Project Overview

The platform is a **controlled-disclosure system for candidate data**. Job seekers register and submit their qualifications, experience, and contact details. The platform operator (Admin) curates filtered batches of these candidates and grants a hiring company **time-limited, scope-limited access** to them — first as anonymised previews, and, upon payment, as full profiles including identity and contact details. Payment is collected offline; the system records the order and controls the resulting entitlement.

The business problem it solves is a matching and trust gap: companies want screened candidate supply without running their own sourcing, and candidates want exposure to employers without publishing their personal details openly.

## 1.3 Scope

### In scope

- Candidate self-registration, profile submission, and document upload
- Candidate profile moderation workflow with a defined lifecycle state machine
- Admin-managed company onboarding (no company self-registration)
- Admin candidate search, filtering, and batch curation
- Exclusive, time-bounded access grants at PREVIEW and FULL scope
- Server-side enforcement of field-level masking by grant scope
- Company portal for reviewing previews, requesting upgrades, and viewing unlocked profiles
- Offline payment recording via Order records that gate grant activation
- Watermarked, access-gated document delivery
- Append-only access logging and a candidate-facing transparency view
- Consent capture, consent versioning, and data-subject request handling
- Outcome tracking with an admin follow-up work queue
- Notifications, dashboards, and operational reporting

### Out of scope

- Online payment processing (payment gateway, invoicing, receipts) — payment is handled offline
- Interview scheduling, applicant tracking, or any post-introduction hiring workflow
- CV parsing / automated resume extraction (Phase 2 candidate)
- Candidate job search or job-posting features — companies do not post jobs on this platform
- Mobile applications (Phase 2 candidate; the web application is responsive)
- Third-party API charges, SMS/email gateway charges, and digital marketing

## 1.4 Definitions & Acronyms

| Term | Definition |
|---|---|
| SRS | Software Requirements Specification |
| API | Application Programming Interface |
| UI / UX | User Interface / User Experience |
| CRUD | Create, Read, Update, Delete |
| LKR | Sri Lankan Rupee |
| PII | Personally Identifiable Information — name, contact details, address, identity documents |
| PDPA | Personal Data Protection Act, No. 9 of 2022 (Sri Lanka) |
| **Candidate** | A registered job seeker whose profile is inventory on the platform |
| **Company** | A hiring organisation, onboarded by Admin after a physical meeting |
| **Batch** | An Admin-curated set of candidates assigned to one Company |
| **Grant** | The entitlement record: one Company may view one Candidate at a defined scope, for a defined period |
| **Scope** | The disclosure level of a Grant — `PREVIEW` (masked) or `FULL` (unmasked) |
| **Preview Profile** | The masked projection of a candidate: category, experience, qualifications, city, skills. No identity, no contact details |
| **Full Profile** | The complete candidate record including PII and original documents |
| **Reservation** | The exclusivity lock — an active Grant makes a Candidate unavailable to all other Companies |
| **Access Event** | An append-only log entry recording a view, reveal, or download |
| **Reference Code** | A public, non-identifying candidate identifier (e.g. `CND-2026-0148`) used in all cross-party communication |

---

# Section 2: User Roles & Permissions

| Role | Description | Key Permissions |
|---|---|---|
| **Admin** | Platform operator / eSupport staff. Full operational control | Manage candidates, moderate profiles, onboard companies, curate batches, issue and upgrade grants, record orders, manage outcomes, view all logs |
| **Company User** | Authorised representative of an onboarded hiring company | View assigned previews, request full access, view unlocked profiles within validity window, download watermarked documents, report outcomes |
| **Candidate** | Registered job seeker | Create and edit own profile, upload documents, view own access log, manage consent, request withdrawal or deletion |

## 2.1 Admin

The Admin role is the operational centre of the system. Admins are the only actors who can create Companies, and the only actors who can create or modify Grants. Every Admin action that changes a Grant, a Company, or a candidate lifecycle state is recorded in the audit log with the acting user's identity.

Two Admin sub-levels are required:

- **Super Admin** — all Admin permissions, plus user management, pricing configuration, masking-rule configuration, and access to the full audit log.
- **Operations Admin** — day-to-day work: moderation, curation, grants, order recording, outcome follow-up. Cannot alter pricing, masking rules, or other Admin accounts.

## 2.2 Company User

Company users are created by Admin, not by self-registration. A Company may have multiple users, each with individual credentials — shared logins are prohibited so that access events remain attributable to a person.

A Company user's visibility is strictly bounded by active Grants issued to their Company. They cannot browse a general candidate pool, cannot search across candidates they have not been granted, and cannot export candidate data in bulk. Access to any candidate ends automatically when the Grant's validity window expires.

## 2.3 Candidate

Candidates self-register and own their data. They may edit their profile at any time, but edits to an Approved profile return it to moderation. Candidates have a right to see who has accessed their data, to withdraw from the platform, and to request deletion — all exercisable from the candidate portal without contacting Admin.

---

# Section 3: Functional Requirements

Requirements use "The system shall…" language. Priority: **High** = must have for launch, **Medium** = should have, **Low** = nice to have / Phase 2.

## 3.1 Authentication & Access Control

| Req ID | Requirement | Priority |
|---|---|---|
| FR-101 | The system shall provide email-and-password authentication for all three roles, with passwords hashed using bcrypt (cost factor ≥ 12) | High |
| FR-102 | The system shall enforce role-based authorisation on every API endpoint, evaluated server-side; UI-level hiding shall not be relied upon for access control | High |
| FR-103 | The system shall require email verification before a Candidate account becomes active | High |
| FR-104 | The system shall require mandatory two-factor authentication (TOTP) for all Admin accounts | High |
| FR-105 | The system shall lock an account for 15 minutes after 5 consecutive failed login attempts from the same IP | High |
| FR-106 | The system shall expire idle sessions after 30 minutes for Admin users and 60 minutes for Company and Candidate users | High |
| FR-107 | The system shall provide a password reset flow using single-use tokens expiring after 60 minutes | High |
| FR-108 | The system shall record every login, logout, and failed login attempt with timestamp, IP address, and user agent | High |
| FR-109 | The system shall allow Admin to suspend or reactivate any Company or Candidate account, immediately invalidating active sessions | High |
| FR-110 | The system shall offer optional two-factor authentication for Company users | Medium |

## 3.2 Candidate Registration & Profile Management

| Req ID | Requirement | Priority |
|---|---|---|
| FR-201 | The system shall allow a Candidate to self-register via a public registration form | High |
| FR-202 | The system shall capture, as **preview-scope** data: job category, desired position, total years of experience, highest qualification, qualification list, skills, main city, availability, and expected salary range | High |
| FR-203 | The system shall capture, as **PII-scope** data: full name, NIC number, email, mobile number, address, date of birth, and profile photo | High |
| FR-204 | The system shall store preview-scope and PII-scope data in physically separate tables with separate access paths | High |
| FR-205 | The system shall assign each Candidate a unique, non-sequential public Reference Code on registration | High |
| FR-206 | The system shall require explicit, recorded consent before a profile may be submitted for review (see 3.12) | High |
| FR-207 | The system shall allow a Candidate to upload up to 5 documents (CV, certificates, references), each ≤ 5 MB, restricted to PDF, DOC, DOCX, JPG, PNG | High |
| FR-208 | The system shall allow a Candidate to edit their profile at any time; edits to an Approved or Published profile shall return it to `Under Review` | High |
| FR-209 | The system shall create an immutable version snapshot of a profile each time it is Approved | High |
| FR-210 | The system shall prevent a Candidate from editing or deleting their profile while it is `Reserved` under an active FULL Grant; edits shall be queued and applied on Grant expiry, with the Candidate notified | High |
| FR-211 | The system shall validate that a Candidate is at least 18 years old at registration | High |
| FR-212 | The system shall prevent duplicate registrations by enforcing uniqueness on NIC number and email address | High |
| FR-213 | The system shall calculate and display a profile completeness percentage to encourage full submissions | Medium |
| FR-214 | The system shall allow a Candidate to set their profile to `Unavailable` (temporarily not seeking work) without deleting it | Medium |
| FR-215 | The system shall support multiple job-category tagging per candidate, up to 3 categories | Medium |

## 3.3 Candidate Moderation & Lifecycle

The candidate lifecycle state machine:

```
Submitted → Under Review → Approved → Published ⇄ Reserved → Placed
                  ↓             ↓          ↓          ↓
               Rejected     Rejected   Withdrawn / Expired
```

| Req ID | Requirement | Priority |
|---|---|---|
| FR-301 | The system shall implement the candidate lifecycle states: `Draft`, `Submitted`, `Under Review`, `Approved`, `Published`, `Reserved`, `Placed`, `Withdrawn`, `Rejected`, `Expired` | High |
| FR-302 | The system shall permit only the transitions defined in the state machine, rejecting any invalid transition at the service layer | High |
| FR-303 | The system shall place a newly submitted profile into `Submitted` and surface it in the Admin moderation queue | High |
| FR-304 | The system shall require an Admin to record a verification note and reviewer identity when moving a profile to `Approved` or `Rejected` | High |
| FR-305 | The system shall require a rejection reason, selected from a configurable list plus free text, when rejecting a profile, and shall notify the Candidate | High |
| FR-306 | The system shall derive the `Reserved` state automatically from the existence of an active Grant; `Reserved` shall not be settable manually | High |
| FR-307 | The system shall return a Candidate from `Reserved` to `Published` automatically when the last active Grant on that Candidate lapses or is closed | High |
| FR-308 | The system shall automatically transition a `Published` profile to `Expired` after 180 days without Candidate login or profile update | High |
| FR-309 | The system shall notify a Candidate 14 days and 3 days before their profile expires, offering a one-click refresh | High |
| FR-310 | The system shall exclude all profiles not in `Published` state from Admin batch-curation search results | High |
| FR-311 | The system shall allow an Admin to bulk-approve profiles from the moderation queue after individual review | Medium |
| FR-312 | The system shall record every lifecycle transition with timestamp, actor, previous state, and new state | High |

## 3.4 Company Management

| Req ID | Requirement | Priority |
|---|---|---|
| FR-401 | The system shall permit Company records to be created only by an Admin; no public company registration endpoint shall exist | High |
| FR-402 | The system shall capture on Company creation: company name, business registration number, industry, address, primary contact person, contact email, contact phone | High |
| FR-403 | The system shall capture and store the onboarding meeting date and the Admin who conducted the physical meeting as mandatory fields | High |
| FR-404 | The system shall allow an Admin to attach verification documents (business registration certificate, signed agreement) to a Company record | High |
| FR-405 | The system shall enforce uniqueness on business registration number | High |
| FR-406 | The system shall allow an Admin to create multiple Company Users under one Company, each with a unique email | High |
| FR-407 | The system shall issue Company User credentials via a single-use activation link, not by emailing a plaintext password | High |
| FR-408 | The system shall support Company statuses: `Active`, `Suspended`, `Terminated`; non-Active companies shall have all access immediately revoked | High |
| FR-409 | The system shall display, on the Company record, a full history of batches, grants, orders, and outcomes for that Company | High |
| FR-410 | The system shall record the signed non-disclosure / data-handling agreement reference on the Company record before any Grant may be issued | High |

## 3.5 Batch Curation & Grant Management

This is the commercial core of the system. **Grants are exclusive**: a Candidate under an active Grant is reserved to that Company and is invisible to every other Company for the duration of the Grant.

| Req ID | Requirement | Priority |
|---|---|---|
| FR-501 | The system shall allow an Admin to search and filter `Published` candidates by job category, years of experience, qualification, city, skills, availability, and expected salary range | High |
| FR-502 | The system shall clearly indicate, in curation search results, which candidates are currently `Reserved` and unavailable for selection | High |
| FR-503 | The system shall allow an Admin to select candidates from filtered results and assemble them into a Batch assigned to exactly one Company | High |
| FR-504 | The system shall require the Admin to set a validity window (`valid_from`, `valid_until`) and scope for the Batch at creation | High |
| FR-505 | The system shall create one Grant record per (Batch, Candidate) pair, each carrying its own scope, validity window, and status | High |
| FR-506 | The system shall enforce, at the database level via a partial unique index, that a Candidate may have at most one active Grant at any time | High |
| FR-507 | The system shall commit Batch creation as a single atomic transaction; if any candidate in the selection has been reserved concurrently, the entire commit shall fail with a clear conflict message identifying the affected candidates | High |
| FR-508 | The system shall store the filter criteria used to assemble each Batch, for reproducibility and audit | Medium |
| FR-509 | The system shall allow an Admin to upgrade the scope of an individual Grant from `PREVIEW` to `FULL` | High |
| FR-510 | The system shall implement scope upgrade by creating a new Grant row and superseding the previous one, never by mutating the existing row | High |
| FR-511 | The system shall prevent activation of a `FULL` scope Grant unless a linked Order is marked `Payment Received` | High |
| FR-512 | The system shall allow an Admin to extend the validity window of an active Grant, recording the extension as a distinct event with reason | High |
| FR-513 | The system shall allow an Admin to revoke a Grant before expiry, immediately terminating access and recording the reason | High |
| FR-514 | The system shall implement Grant statuses: `Draft`, `Active`, `Superseded`, `Lapsed`, `Revoked`, `Closed` | High |
| FR-515 | The system shall evaluate Grant validity at query time on every request, using `now BETWEEN valid_from AND valid_until AND status = 'Active'`, and shall not rely on a scheduled job as the security boundary | High |
| FR-516 | The system shall run a scheduled job at least hourly to transition expired Grants to `Lapsed` and emit the associated notifications and follow-up tasks | High |
| FR-517 | The system shall notify the assigned Company 3 days before a Grant window closes | Medium |
| FR-518 | The system shall provide the Admin a live dashboard count of Available vs. Reserved candidates per job category | Medium |
| FR-519 | The system shall prevent an Admin from issuing a Grant to a Company whose status is not `Active` | High |

## 3.6 Company Portal — Preview & Full Access

| Req ID | Requirement | Priority |
|---|---|---|
| FR-601 | The system shall present a Company user only with candidates for whom their Company holds a currently valid Grant | High |
| FR-602 | The system shall serve preview-scope requests through a projection that has no join path to the PII table, such that PII cannot be returned even in error | High |
| FR-603 | The system shall display in a Preview Profile only: reference code, job category, position sought, years of experience, qualification summary, skills, main city, availability, and expected salary range | High |
| FR-604 | The system shall apply configured masking rules to preview text fields, redacting employer names, institution names, and any free-text occurrence of the candidate's name (see 3.9) | High |
| FR-605 | The system shall allow a Company user to flag one or more previewed candidates as "Request Full Access", with optional notes | High |
| FR-606 | The system shall notify Admin immediately when a full-access request is submitted | High |
| FR-607 | The system shall display request status to the Company: `Requested`, `Quoted`, `Awaiting Payment`, `Granted`, `Declined` | High |
| FR-608 | The system shall reveal PII only where a valid Grant with scope `FULL` resolves for the requesting Company at request time | High |
| FR-609 | The system shall display, on every profile view, the remaining validity of the Grant | Medium |
| FR-610 | The system shall provide no bulk export, print-all, CSV download, or public API access to candidate data for Company users | High |
| FR-611 | The system shall rate-limit Company profile views to a configurable maximum per user per day (default 100) and alert Admin on breach | High |
| FR-612 | The system shall return a Company user to a clear "access expired" screen, with no cached data rendered, once a Grant lapses | High |
| FR-613 | The system shall allow a Company user to add private notes against a candidate they hold access to; notes shall persist after Grant expiry but shall not retain candidate PII | Medium |

## 3.7 Document Management

Documents are the highest-risk surface in the system: a CV contains every field the preview scope is designed to conceal.

| Req ID | Requirement | Priority |
|---|---|---|
| FR-701 | The system shall store all candidate documents in private object storage with no publicly resolvable URL | High |
| FR-702 | The system shall serve documents exclusively via short-lived signed URLs, expiring within 5 minutes of issue | High |
| FR-703 | The system shall issue a signed URL only after resolving a valid `FULL` scope Grant for the requesting Company and candidate | High |
| FR-704 | The system shall stamp every generated document with a visible watermark containing the requesting Company name, the requesting user's email, and the download timestamp | High |
| FR-705 | The system shall not expose original candidate documents at `PREVIEW` scope under any circumstance | High |
| FR-706 | The system shall generate and serve a redacted CV variant at `PREVIEW` scope, with name, contact details, and employer names removed | Medium |
| FR-707 | The system shall log every document download as an Access Event, including the document identifier and the signed-URL issue time | High |
| FR-708 | The system shall scan all uploaded files for malware before making them available | High |
| FR-709 | The system shall validate uploaded file content type by inspecting file signature, not by trusting the client-supplied extension or MIME type | High |

## 3.8 Order & Payment Recording

Payment is taken offline. The system does not process money, but it must be the authoritative record of what was sold, to whom, and for how much.

| Req ID | Requirement | Priority |
|---|---|---|
| FR-801 | The system shall allow an Admin to create an Order against a Company, containing one line item per candidate to be unlocked | High |
| FR-802 | The system shall record the unit price on the Order line item at the time of creation, so that later price changes do not alter historical records | High |
| FR-803 | The system shall apply a single flat unit price across all candidates regardless of job category, configurable by Super Admin | High |
| FR-804 | The system shall implement Order statuses: `Draft`, `Quoted`, `Awaiting Payment`, `Payment Received`, `Cancelled` | High |
| FR-805 | The system shall require an Admin to record payment reference, payment date, payment method, and confirming Admin identity when marking an Order `Payment Received` | High |
| FR-806 | The system shall prevent any `FULL` scope Grant from becoming `Active` unless its linked Order is in `Payment Received` status | High |
| FR-807 | The system shall generate a printable order summary in PDF for the Company, using LKR as currency | Medium |
| FR-808 | The system shall provide Admin a receivables view listing all Orders in `Awaiting Payment` with ageing in days | Medium |
| FR-809 | The system shall record all Order status changes in the audit log with actor and timestamp | High |

## 3.9 Masking & Anonymisation Rules

Removing name and phone number does not anonymise a profile. In a market the size of Sri Lanka's, a niche job title combined with an employer name and a city is frequently sufficient to identify an individual. Masking must therefore be rule-driven and applied to free text, not only to structured fields.

| Req ID | Requirement | Priority |
|---|---|---|
| FR-901 | The system shall maintain a configurable masking rule set, editable by Super Admin | High |
| FR-902 | The system shall replace employer names in preview experience entries with an industry-and-size descriptor (e.g. "Mid-size fintech, Colombo") | High |
| FR-903 | The system shall replace named educational institutions in preview qualifications with a category descriptor (e.g. "State university — Sri Lanka") | High |
| FR-904 | The system shall scan all preview free-text fields for occurrences of the candidate's own name, NIC, email, or phone number and redact them | High |
| FR-905 | The system shall reduce location precision in preview scope to main city only, excluding street address, suburb, and postal code | High |
| FR-906 | The system shall present the Admin with a preview-as-company view during moderation, so the masked output can be visually verified before publication | High |
| FR-907 | The system shall flag for Admin review any candidate whose combination of job category, experience band, and city matches fewer than 5 other published candidates, as a re-identification risk | Medium |

## 3.10 Access Logging & Candidate Transparency

| Req ID | Requirement | Priority |
|---|---|---|
| FR-1001 | The system shall write an append-only Access Event for every profile view, field reveal, and document download | High |
| FR-1002 | The system shall record on each Access Event: candidate, company, company user, grant, scope, profile version shown, timestamp, IP address, and user agent | High |
| FR-1003 | The system shall prevent modification or deletion of Access Events by any role, including Super Admin | High |
| FR-1004 | The system shall provide each Candidate a transparency view in their portal showing how many companies have previewed their profile and how many hold full access | High |
| FR-1005 | The system shall disclose to the Candidate the identity of any Company that has been granted `FULL` access to their profile | High |
| FR-1006 | The system shall notify a Candidate by email when a Company is granted full access to their profile | High |
| FR-1007 | The system shall provide Admin an anomaly report highlighting unusual access patterns, such as a Company user viewing the maximum permitted profiles on consecutive days | Medium |
| FR-1008 | The system shall retain Access Events for a minimum of 24 months | High |

## 3.11 Outcome Tracking & Follow-Up

| Req ID | Requirement | Priority |
|---|---|---|
| FR-1101 | The system shall allow a Company user to report an outcome against each candidate for which they hold a `FULL` Grant | High |
| FR-1102 | The system shall support outcome values: `Hired`, `Interview Pending`, `Rejected`, `Candidate Declined`, `No Response`, `Not Contacted` | High |
| FR-1103 | The system shall prompt the Company for an outcome 3 days before a `FULL` Grant expires and again on expiry | High |
| FR-1104 | The system shall generate an Admin follow-up task, with a due date, when a `FULL` Grant lapses without a reported outcome | High |
| FR-1105 | The system shall allow an Admin to record an outcome on the Company's behalf, capturing the contact method and date of the conversation | High |
| FR-1106 | The system shall allow an `Interview Pending` outcome to trigger a Grant extension request rather than forcing a premature final answer | High |
| FR-1107 | The system shall transition a Candidate to `Placed` when an outcome of `Hired` is confirmed by an Admin | High |
| FR-1108 | The system shall return a Candidate to `Published` on any outcome other than `Hired`, once the Grant has lapsed | High |
| FR-1109 | The system shall notify the Candidate and request confirmation when they are marked `Placed` | Medium |
| FR-1110 | The system shall escalate a follow-up task to Super Admin if it remains open 7 days past its due date | Medium |

## 3.12 Consent & Data Protection

Consent is treated as an architectural component, not a legal footnote. Sri Lanka's PDPA applies directly to this processing, and consent is commercially aligned here — candidates register precisely because they want employers to see them.

| Req ID | Requirement | Priority |
|---|---|---|
| FR-1201 | The system shall capture explicit, affirmative consent before a candidate profile may be submitted, using an unchecked-by-default control | High |
| FR-1202 | The system shall record on each Consent: the terms version accepted, timestamp, IP address, and consent scope | High |
| FR-1203 | The system shall version the terms of service and privacy notice, and shall re-request consent from all candidates when a material change is published | High |
| FR-1204 | The system shall provide a Candidate a one-click withdrawal action that immediately halts all new Grant issuance for that candidate | High |
| FR-1205 | The system shall handle withdrawal during an active `FULL` Grant by allowing the existing Grant to run to expiry while blocking all new Grants, and shall notify both the Candidate and the Company of the withdrawal | High |
| FR-1206 | The system shall provide a Candidate a data export of all data held about them, in machine-readable format | High |
| FR-1207 | The system shall provide a Candidate a deletion request path, resolved by Admin within 30 days, retaining only the minimum transactional record required to evidence past lawful processing | High |
| FR-1208 | The system shall anonymise rather than hard-delete candidate records referenced by settled Orders, replacing PII with irreversible tokens while retaining the reference code | High |
| FR-1209 | The system shall maintain a data-processing register recording purpose, legal basis, retention period, and recipients for each category of personal data | High |
| FR-1210 | The system shall require Company users to accept a data-handling undertaking at first login, recorded with version and timestamp | High |

## 3.13 Notifications

| Req ID | Requirement | Priority |
|---|---|---|
| FR-1301 | The system shall send transactional email notifications for: registration verification, profile approval, profile rejection, expiry warning, full-access grant to a company, and placement confirmation | High |
| FR-1302 | The system shall notify Admin for: new profile submission, full-access request, order payment overdue, grant lapsed without outcome, and rate-limit breach | High |
| FR-1303 | The system shall notify Company users for: new batch assigned, access request approved, grant expiring, and outcome reminder | High |
| FR-1304 | The system shall maintain an in-application notification centre for all roles with read/unread state | Medium |
| FR-1305 | The system shall queue all outbound notifications and retry failed sends up to 3 times with exponential backoff | High |
| FR-1306 | The system shall support SMS notification for candidates as a configurable channel | Low |

## 3.14 Reporting & Dashboards

| Req ID | Requirement | Priority |
|---|---|---|
| FR-1401 | The system shall provide an Admin dashboard showing: total published candidates, candidates by category, available vs. reserved counts, active grants, expiring grants, outstanding orders, and open follow-up tasks | High |
| FR-1402 | The system shall provide a conversion report tracking preview-to-full-access conversion rate by company and by job category | Medium |
| FR-1403 | The system shall provide a placement report showing hires by company, by category, and by period | Medium |
| FR-1404 | The system shall provide a revenue report by period, based on Orders in `Payment Received` status, in LKR | Medium |
| FR-1405 | The system shall provide an inventory ageing report showing published candidates by days since last activity | Medium |
| FR-1406 | The system shall allow Admin to export reports as CSV; report exports shall never include candidate PII unless explicitly authorised by Super Admin and logged | High |

---

# Section 4: Non-Functional Requirements

| Category | Requirement |
|---|---|
| **Performance** | Page load under 3 seconds on standard broadband; API responses under 500 ms at the 95th percentile for read operations |
| **Scalability** | Support 500 concurrent users and a pool of 100,000 candidate profiles without architectural change; candidate search shall remain under 1 second at that volume |
| **Security — transport** | All traffic over HTTPS/TLS 1.2+; HSTS enabled; no mixed content |
| **Security — storage** | Passwords hashed with bcrypt (cost ≥ 12); PII columns encrypted at rest using AES-256; encryption keys held in a managed key store, never in source control |
| **Security — access** | Grant validity evaluated server-side at query time on every request; no client-side enforcement of masking |
| **Security — application** | Protection against OWASP Top 10; parameterised queries only; CSRF tokens on all state-changing requests; strict Content-Security-Policy |
| **Auditability** | All access to candidate PII logged immutably; logs retained 24 months minimum |
| **Availability** | 99.5% uptime target, excluding scheduled maintenance windows announced 48 hours in advance |
| **Backup** | Automated daily database backups with 30-day retention; documented restore procedure tested quarterly; object storage versioning enabled |
| **Compatibility** | Responsive across desktop, tablet, and mobile; supports current and prior major versions of Chrome, Firefox, Safari, and Edge |
| **Maintainability** | Backend follows MVC with a service layer; all APIs documented via OpenAPI; automated test coverage ≥ 70% on access-control and grant-resolution logic |
| **Data residency** | Personal data stored in a jurisdiction consistent with PDPA obligations; any cross-border transfer documented in the processing register |
| **Accessibility** | Candidate-facing pages meet WCAG 2.1 Level AA |
| **Localisation** | English at launch; the UI string layer shall be externalised to permit Sinhala and Tamil in a later phase |
| **Rate limiting** | Per-user and per-IP rate limits on authentication, search, and profile-view endpoints |

---

# Section 5: System Architecture

## 5.1 Technology Stack

| Layer | Technology |
|---|---|
| Frontend | React / Next.js |
| Backend | Laravel (PHP) — REST API |
| Database | MySQL 8 |
| Object storage | Cloud object storage (private buckets, signed URLs) |
| Cache / queue | Redis — session cache, rate limiting, job queue |
| Background jobs | Laravel Queue workers + scheduler |
| Mobile | React Native (Phase 2, if required) |
| Hosting | Cloud Hosting |
| Version control | Git / GitHub (eSupportTechnology organisation) |

## 5.2 Architecture Overview

The system follows a RESTful API architecture with a service layer between controllers and data access. The frontend is a Next.js application communicating exclusively over HTTPS REST calls; it holds no business logic relating to access control.

The defining architectural characteristic is the **dual-projection data access path**. Candidate data is split at the persistence layer into a preview repository and a PII repository. The preview repository's queries are structurally incapable of joining to the PII tables. Which repository is used is decided by a Grant Resolver service, invoked before any candidate data is read. This means an access-control defect in a controller cannot leak PII, because the code path that would return it was never reachable.

```
        [Candidate Browser]   [Company Browser]   [Admin Browser]
                 |                    |                  |
                 +---------- HTTPS ---+------------------+
                                      |
                        [Frontend: Next.js / React]
                                      |
                              REST API (JSON)
                                      |
                   +------------------------------------+
                   |   Backend: Laravel API             |
                   |                                    |
                   |   Auth & RBAC Middleware           |
                   |            |                       |
                   |   Grant Resolver Service           |
                   |       /            \               |
                   |  Preview          Full             |
                   |  Projection       Projection       |
                   |  Repository       Repository       |
                   |       |                |           |
                   |  Masking Engine        |           |
                   |       |                |           |
                   |   Access Event Writer (append-only)|
                   +------------------------------------+
                          |          |            |
                   [MySQL]      [Redis]    [Object Storage]
                   profiles     sessions    documents
                   PII (enc.)   queue       (signed URL only)
                   grants       rate limit
                   access log
```

## 5.3 Access Enforcement Layers

Three independent layers protect candidate PII. All three are required; none is sufficient alone.

**Layer 1 — Query-time validity.** The Grant Resolver evaluates `now BETWEEN valid_from AND valid_until AND status = 'Active'` on every single request. Scheduled jobs that flip Grant statuses exist for notification and reporting purposes only. If a scheduled job were the security boundary, one failed cron run would become a data breach.

**Layer 2 — Physical projection separation.** `PreviewProfileDTO` is assembled by a query that never touches `candidate_pii`. `FullProfileDTO` requires a resolved `FULL` Grant. There is no single query with conditional field-stripping in the response serialiser.

**Layer 3 — Gated document delivery.** Documents are never served as direct storage links. Signed URLs are issued only against a resolved `FULL` Grant, expire in minutes, and every issued URL is logged.

## 5.4 Concurrency Control for Exclusive Reservation

Because a Candidate may hold only one active Grant, batch curation is a reservation operation and is subject to race conditions when two Admins curate simultaneously.

The control is a **partial unique index** on the grants table covering `candidate_id` where `status = 'Active'`. Batch commit runs inside a single database transaction; if any candidate in the selection has been reserved by a concurrent transaction, the insert violates the index and the entire batch commit rolls back. The Admin receives a conflict message naming the affected candidates and may re-curate. Application-level "check then insert" logic is explicitly insufficient and shall not be used as the primary control.

---

# Section 6: Database / Data Models

Field lists below give the principal columns; standard `created_at` and `updated_at` timestamps apply to all tables and are shown only where noteworthy. All tables use soft deletes except `access_events`, which is strictly append-only.

## 6.1 users

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Auto-increment primary key |
| uuid | CHAR(36) | External identifier used in URLs |
| email | VARCHAR(255) UNIQUE | Login identifier |
| password_hash | VARCHAR(255) | bcrypt hash |
| role | ENUM | `super_admin`, `ops_admin`, `company_user`, `candidate` |
| company_id | BIGINT (FK, nullable) | Set for company users only |
| status | ENUM | `pending`, `active`, `suspended` |
| two_factor_secret | VARCHAR(255) nullable | TOTP secret, encrypted |
| email_verified_at | TIMESTAMP nullable | Verification timestamp |
| last_login_at | TIMESTAMP nullable | Most recent successful login |

*Relationships:* belongs to Company (nullable); has one Candidate Profile (for candidate role).

## 6.2 candidate_profiles — preview scope

Contains only data that is safe to disclose at PREVIEW scope after masking.

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| user_id | BIGINT (FK) | Owning user account |
| reference_code | VARCHAR(20) UNIQUE | Public non-identifying code, e.g. `CND-2026-0148` |
| job_category_id | BIGINT (FK) | Primary job category |
| position_sought | VARCHAR(150) | Desired role title |
| years_experience | DECIMAL(4,1) | Total years of experience |
| experience_band | ENUM | `0-1`, `1-3`, `3-5`, `5-10`, `10+` — used for search and re-identification risk |
| highest_qualification | VARCHAR(150) | Highest qualification held |
| main_city | VARCHAR(100) | City only — no street or postal detail |
| availability | ENUM | `immediate`, `1_month`, `2_months`, `3_months_plus` |
| expected_salary_min | DECIMAL(12,2) nullable | LKR |
| expected_salary_max | DECIMAL(12,2) nullable | LKR |
| status | ENUM | Lifecycle state (see 3.3) |
| completeness_pct | TINYINT | Calculated completeness score |
| last_activity_at | TIMESTAMP | Drives 180-day expiry |
| published_at | TIMESTAMP nullable | First publication timestamp |

*Relationships:* belongs to User; belongs to Job Category; has one Candidate PII; has many Qualifications, Experiences, Skills, Documents, Grants, Access Events, Profile Versions.

## 6.3 candidate_pii — restricted scope

Physically separate table. Accessible only through the Full Projection Repository.

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK, UNIQUE) | One-to-one with profile |
| full_name | VARBINARY | AES-256 encrypted |
| nic_number | VARBINARY UNIQUE (hashed index) | Encrypted; blind index for uniqueness |
| email | VARBINARY | Encrypted |
| mobile | VARBINARY | Encrypted |
| address_line_1 | VARBINARY | Encrypted |
| address_line_2 | VARBINARY nullable | Encrypted |
| postal_code | VARCHAR(10) nullable | |
| date_of_birth | DATE | Used for 18+ validation |
| photo_path | VARCHAR(255) nullable | Private storage key |

*Relationships:* belongs to Candidate Profile.

## 6.4 candidate_experiences

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK) | Owning profile |
| employer_name | VARCHAR(200) | Full name — PII-adjacent, masked in preview |
| employer_descriptor | VARCHAR(200) | Masked substitute, e.g. "Mid-size fintech, Colombo" |
| job_title | VARCHAR(150) | Role held |
| industry | VARCHAR(100) | Industry classification |
| start_date | DATE | |
| end_date | DATE nullable | Null indicates current role |
| responsibilities | TEXT | Free text — masking engine applied before preview |

*Relationships:* belongs to Candidate Profile.

## 6.5 candidate_qualifications

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK) | Owning profile |
| qualification_name | VARCHAR(200) | e.g. "BSc (Hons) Computer Science" |
| institution_name | VARCHAR(200) | Full name — masked in preview |
| institution_descriptor | VARCHAR(200) | Masked substitute, e.g. "State university — Sri Lanka" |
| level | ENUM | `certificate`, `diploma`, `bachelors`, `masters`, `doctorate`, `professional` |
| year_completed | YEAR | |

*Relationships:* belongs to Candidate Profile.

## 6.6 candidate_documents

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK) | Owning profile |
| document_type | ENUM | `cv`, `certificate`, `reference`, `other` |
| storage_key | VARCHAR(500) | Private object storage key — never exposed to client |
| redacted_storage_key | VARCHAR(500) nullable | Preview-safe redacted variant |
| original_filename | VARCHAR(255) | As uploaded |
| mime_type | VARCHAR(100) | Verified by file signature |
| file_size_bytes | INT | |
| scan_status | ENUM | `pending`, `clean`, `infected` |

*Relationships:* belongs to Candidate Profile; has many Access Events.

## 6.7 profile_versions

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK) | Owning profile |
| version_number | INT | Sequential per profile |
| snapshot_json | JSON | Immutable full snapshot at approval |
| approved_by | BIGINT (FK users) | Approving Admin |
| approved_at | TIMESTAMP | |

*Relationships:* belongs to Candidate Profile; referenced by Access Events.

Rationale: a Company that says "this is not the profile we paid for" must be answerable with evidence of exactly what was displayed and when.

## 6.8 companies

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| name | VARCHAR(200) | Registered company name |
| business_reg_no | VARCHAR(50) UNIQUE | Business registration number |
| industry | VARCHAR(100) | |
| address | VARCHAR(500) | |
| contact_person | VARCHAR(150) | Primary contact |
| contact_email | VARCHAR(255) | |
| contact_phone | VARCHAR(30) | |
| onboarding_meeting_date | DATE | Mandatory — date of physical meeting |
| onboarded_by | BIGINT (FK users) | Admin who conducted the meeting |
| agreement_reference | VARCHAR(100) | Signed data-handling agreement reference |
| status | ENUM | `active`, `suspended`, `terminated` |

*Relationships:* has many Users, Batches, Grants, Orders, Access Events.

## 6.9 batches

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| company_id | BIGINT (FK) | Assigned company — exactly one |
| batch_code | VARCHAR(30) UNIQUE | e.g. `BAT-2026-0071` |
| title | VARCHAR(200) | Admin-facing label |
| filter_criteria_json | JSON | Filters used to assemble the batch |
| default_valid_from | TIMESTAMP | |
| default_valid_until | TIMESTAMP | |
| created_by | BIGINT (FK users) | Curating Admin |
| status | ENUM | `draft`, `issued`, `closed` |

*Relationships:* belongs to Company; has many Grants.

## 6.10 grants

The central entity of the system.

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| batch_id | BIGINT (FK) | Parent batch |
| company_id | BIGINT (FK) | Denormalised for query-time resolution |
| candidate_profile_id | BIGINT (FK) | Granted candidate |
| scope | ENUM | `preview`, `full` |
| status | ENUM | `draft`, `active`, `superseded`, `lapsed`, `revoked`, `closed` |
| valid_from | TIMESTAMP | Window start |
| valid_until | TIMESTAMP | Window end |
| order_id | BIGINT (FK, nullable) | Required and must be paid for `full` scope |
| supersedes_grant_id | BIGINT (FK, nullable) | Set on upgrade — preserves history |
| issued_by | BIGINT (FK users) | Issuing Admin |
| revoked_by | BIGINT (FK users) nullable | |
| revocation_reason | VARCHAR(500) nullable | |
| extension_count | TINYINT | Number of window extensions applied |

*Relationships:* belongs to Batch, Company, Candidate Profile, Order; has many Access Events; has one Outcome.

**Constraints and indexes:**
- Partial unique index on `(candidate_profile_id)` where `status = 'active'` — enforces exclusive reservation (FR-506)
- Composite index on `(company_id, candidate_profile_id, status, valid_until)` — grant resolution hot path
- Check constraint: `valid_until > valid_from`
- Application-level invariant: `scope = 'full'` requires `order_id IS NOT NULL` and that order in `payment_received`

## 6.11 orders

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| company_id | BIGINT (FK) | Purchasing company |
| order_code | VARCHAR(30) UNIQUE | e.g. `ORD-2026-0219` |
| status | ENUM | `draft`, `quoted`, `awaiting_payment`, `payment_received`, `cancelled` |
| subtotal | DECIMAL(12,2) | LKR |
| total | DECIMAL(12,2) | LKR |
| payment_reference | VARCHAR(100) nullable | Bank reference / cheque number |
| payment_method | ENUM nullable | `bank_transfer`, `cheque`, `cash` |
| payment_date | DATE nullable | |
| confirmed_by | BIGINT (FK users) nullable | Admin who confirmed receipt |

*Relationships:* belongs to Company; has many Order Items; has many Grants.

## 6.12 order_items

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| order_id | BIGINT (FK) | Parent order |
| candidate_profile_id | BIGINT (FK) | Candidate being unlocked |
| unit_price | DECIMAL(12,2) | Price captured at creation — never read from config at display time |
| access_days | SMALLINT | Duration of the full-access window purchased |

*Relationships:* belongs to Order and Candidate Profile.

## 6.13 access_events — append-only

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| grant_id | BIGINT (FK) | Governing grant |
| company_id | BIGINT (FK) | Accessing company |
| user_id | BIGINT (FK) | Accessing user — individual, not shared |
| candidate_profile_id | BIGINT (FK) | Subject candidate |
| profile_version_id | BIGINT (FK) | Exact version displayed |
| event_type | ENUM | `preview_view`, `full_view`, `field_reveal`, `document_download`, `signed_url_issued` |
| document_id | BIGINT (FK, nullable) | Set for document events |
| ip_address | VARCHAR(45) | IPv4 or IPv6 |
| user_agent | VARCHAR(500) | |
| occurred_at | TIMESTAMP | Event time |

*Relationships:* belongs to Grant, Company, User, Candidate Profile, Profile Version.

No UPDATE or DELETE permission is granted on this table to the application database user.

## 6.14 consents

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| candidate_profile_id | BIGINT (FK) | Consenting candidate |
| terms_version | VARCHAR(20) | Version of terms accepted |
| privacy_version | VARCHAR(20) | Version of privacy notice accepted |
| consent_scope | JSON | Which processing purposes were agreed |
| granted_at | TIMESTAMP | |
| withdrawn_at | TIMESTAMP nullable | Set on withdrawal |
| ip_address | VARCHAR(45) | Evidence of the consenting action |

*Relationships:* belongs to Candidate Profile.

## 6.15 outcomes

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| grant_id | BIGINT (FK, UNIQUE) | One outcome per full grant |
| outcome | ENUM | `hired`, `interview_pending`, `rejected`, `candidate_declined`, `no_response`, `not_contacted` |
| reported_by | BIGINT (FK users) | Company user or Admin |
| reported_via | ENUM | `company_portal`, `admin_manual` |
| contact_method | VARCHAR(50) nullable | For admin-recorded outcomes |
| notes | TEXT nullable | |
| confirmed_by_admin | BOOLEAN | Required before `Placed` transition |

*Relationships:* belongs to Grant.

## 6.16 follow_up_tasks

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| grant_id | BIGINT (FK) | Related grant |
| assigned_to | BIGINT (FK users) | Responsible Admin |
| task_type | ENUM | `outcome_chase`, `payment_chase`, `verification`, `escalation` |
| due_date | DATE | |
| status | ENUM | `open`, `in_progress`, `done`, `escalated` |
| resolution_notes | TEXT nullable | |

*Relationships:* belongs to Grant and User.

## 6.17 masking_rules

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| rule_type | ENUM | `employer_name`, `institution_name`, `self_reference`, `location_precision`, `custom_regex` |
| pattern | VARCHAR(500) nullable | For custom rules |
| replacement_strategy | ENUM | `descriptor`, `redact`, `generalise` |
| is_active | BOOLEAN | |

## 6.18 audit_log

| Field | Type | Description |
|---|---|---|
| id | BIGINT (PK) | Primary key |
| actor_user_id | BIGINT (FK) | Acting user |
| entity_type | VARCHAR(100) | e.g. `Grant`, `Order`, `CandidateProfile` |
| entity_id | BIGINT | Affected record |
| action | VARCHAR(100) | e.g. `grant.upgraded`, `order.payment_confirmed` |
| before_json | JSON nullable | Prior state |
| after_json | JSON nullable | New state |
| ip_address | VARCHAR(45) | |
| occurred_at | TIMESTAMP | |

## 6.19 Supporting tables

| Table | Purpose |
|---|---|
| job_categories | Reference list: HR Intern, Associate SE, Junior Executive, General Manager, etc. Hierarchical via `parent_id` |
| skills | Normalised skill list |
| candidate_skills | Many-to-many join between candidates and skills, with proficiency level |
| candidate_categories | Many-to-many join supporting up to 3 categories per candidate (FR-215) |
| company_notes | Private company notes against candidates; retains no PII after grant expiry |
| access_requests | Company requests for scope upgrade, with status and admin response |
| notifications | In-app notification centre records with read state |
| settings | Configurable values: unit price, default window length, rate limits, expiry thresholds |

---

# Section 7: API Endpoints

The following REST API endpoints will be implemented. All endpoints are prefixed `/api/v1`. All authenticated endpoints require a bearer token. **Role** indicates the minimum role required.

## 7.1 Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| POST | /auth/register | Candidate self-registration | No |
| POST | /auth/verify-email | Confirm email with token | No |
| POST | /auth/login | Authenticate and issue token | No |
| POST | /auth/2fa/verify | Submit TOTP code (mandatory for admin) | Partial |
| POST | /auth/logout | Invalidate current session | Yes |
| POST | /auth/password/forgot | Request reset link | No |
| POST | /auth/password/reset | Complete reset with token | No |
| POST | /auth/activate | Company user activation via single-use link | No |
| GET | /auth/me | Current user and role context | Yes |

## 7.2 Candidate Profile Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /candidate/profile | Retrieve own full profile | Yes (Candidate) |
| PUT | /candidate/profile | Update own profile — returns it to Under Review | Yes (Candidate) |
| POST | /candidate/profile/submit | Submit for moderation | Yes (Candidate) |
| GET | /candidate/profile/preview-as-company | See own masked preview as a company would | Yes (Candidate) |
| POST | /candidate/experiences | Add experience entry | Yes (Candidate) |
| PUT | /candidate/experiences/{id} | Update experience entry | Yes (Candidate) |
| DELETE | /candidate/experiences/{id} | Remove experience entry | Yes (Candidate) |
| POST | /candidate/qualifications | Add qualification | Yes (Candidate) |
| PUT | /candidate/qualifications/{id} | Update qualification | Yes (Candidate) |
| DELETE | /candidate/qualifications/{id} | Remove qualification | Yes (Candidate) |
| POST | /candidate/documents | Upload document | Yes (Candidate) |
| DELETE | /candidate/documents/{id} | Remove document | Yes (Candidate) |
| PATCH | /candidate/availability | Toggle available / unavailable | Yes (Candidate) |
| POST | /candidate/refresh | One-click activity refresh to prevent expiry | Yes (Candidate) |

## 7.3 Candidate Transparency & Rights Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /candidate/access-log | Companies that previewed or unlocked own profile | Yes (Candidate) |
| GET | /candidate/consents | Consent history | Yes (Candidate) |
| POST | /candidate/consents | Accept current terms version | Yes (Candidate) |
| POST | /candidate/withdraw | Withdraw from platform — blocks new grants | Yes (Candidate) |
| POST | /candidate/data-export | Request machine-readable export of own data | Yes (Candidate) |
| POST | /candidate/deletion-request | Submit deletion request | Yes (Candidate) |

## 7.4 Admin — Moderation Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /admin/moderation/queue | List profiles awaiting review | Yes (Admin) |
| GET | /admin/candidates/{id} | Full candidate record for verification | Yes (Admin) |
| GET | /admin/candidates/{id}/preview | Masked preview verification view | Yes (Admin) |
| POST | /admin/candidates/{id}/approve | Approve — creates a profile version snapshot | Yes (Admin) |
| POST | /admin/candidates/{id}/reject | Reject with reason | Yes (Admin) |
| POST | /admin/candidates/{id}/publish | Move Approved to Published | Yes (Admin) |
| PATCH | /admin/candidates/{id}/status | Explicit lifecycle transition | Yes (Admin) |
| GET | /admin/candidates/{id}/versions | Version history | Yes (Admin) |

## 7.5 Admin — Company Management Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /admin/companies | List companies with filters | Yes (Admin) |
| POST | /admin/companies | Create company after physical meeting | Yes (Admin) |
| GET | /admin/companies/{id} | Company detail with grant and order history | Yes (Admin) |
| PUT | /admin/companies/{id} | Update company record | Yes (Admin) |
| PATCH | /admin/companies/{id}/status | Activate / suspend / terminate | Yes (Admin) |
| POST | /admin/companies/{id}/users | Create a company user | Yes (Admin) |
| DELETE | /admin/companies/{id}/users/{userId} | Deactivate a company user | Yes (Admin) |
| POST | /admin/companies/{id}/documents | Attach verification documents | Yes (Admin) |

## 7.6 Admin — Curation & Grant Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /admin/candidates/search | Filter published candidates; flags reserved availability | Yes (Admin) |
| GET | /admin/candidates/availability-summary | Available vs. reserved counts by category | Yes (Admin) |
| POST | /admin/batches | Create a draft batch | Yes (Admin) |
| GET | /admin/batches | List batches | Yes (Admin) |
| GET | /admin/batches/{id} | Batch detail with grants | Yes (Admin) |
| PUT | /admin/batches/{id} | Amend a draft batch | Yes (Admin) |
| POST | /admin/batches/{id}/commit | **Atomic** issue — reserves all candidates or fails wholesale | Yes (Admin) |
| DELETE | /admin/batches/{id} | Discard a draft batch | Yes (Admin) |
| GET | /admin/grants | List grants with status and expiry filters | Yes (Admin) |
| POST | /admin/grants/{id}/upgrade | Upgrade PREVIEW to FULL — creates a superseding grant | Yes (Admin) |
| POST | /admin/grants/{id}/extend | Extend validity window with reason | Yes (Admin) |
| POST | /admin/grants/{id}/revoke | Revoke immediately with reason | Yes (Admin) |
| GET | /admin/access-requests | Pending company upgrade requests | Yes (Admin) |
| PATCH | /admin/access-requests/{id} | Quote, approve, or decline a request | Yes (Admin) |

## 7.7 Admin — Order Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /admin/orders | List orders with status filters | Yes (Admin) |
| POST | /admin/orders | Create order with line items | Yes (Admin) |
| GET | /admin/orders/{id} | Order detail | Yes (Admin) |
| PATCH | /admin/orders/{id}/status | Advance order status | Yes (Admin) |
| POST | /admin/orders/{id}/confirm-payment | Record offline payment receipt | Yes (Admin) |
| GET | /admin/orders/{id}/summary-pdf | Printable order summary in LKR | Yes (Admin) |
| GET | /admin/receivables | Awaiting-payment orders with ageing | Yes (Admin) |

## 7.8 Admin — Outcome & Operations Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /admin/follow-ups | Open follow-up task queue | Yes (Admin) |
| PATCH | /admin/follow-ups/{id} | Update or close a task | Yes (Admin) |
| POST | /admin/grants/{id}/outcome | Record outcome on company's behalf | Yes (Admin) |
| POST | /admin/outcomes/{id}/confirm | Confirm outcome; triggers Placed transition on hire | Yes (Admin) |
| GET | /admin/reports/conversion | Preview-to-full conversion metrics | Yes (Admin) |
| GET | /admin/reports/placements | Placement report | Yes (Admin) |
| GET | /admin/reports/revenue | Revenue by period, LKR | Yes (Admin) |
| GET | /admin/reports/inventory-ageing | Stale inventory report | Yes (Admin) |
| GET | /admin/access-anomalies | Unusual company access patterns | Yes (Admin) |
| GET | /admin/audit-log | Filterable audit trail | Yes (Super Admin) |
| GET | /admin/settings | Read configuration | Yes (Admin) |
| PUT | /admin/settings | Update pricing, windows, rate limits | Yes (Super Admin) |
| GET | /admin/masking-rules | List masking rules | Yes (Super Admin) |
| PUT | /admin/masking-rules/{id} | Update masking rule | Yes (Super Admin) |

## 7.9 Company Portal Endpoints

Every endpoint below resolves the Grant before returning any data. No endpoint accepts an arbitrary candidate identifier without grant resolution.

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | /company/batches | Batches assigned to own company | Yes (Company) |
| GET | /company/candidates | Candidates accessible under valid grants, with scope indicator | Yes (Company) |
| GET | /company/candidates/{referenceCode} | Profile at the resolved scope — masked or full | Yes (Company) |
| POST | /company/access-requests | Request full access for selected candidates | Yes (Company) |
| GET | /company/access-requests | Own request statuses | Yes (Company) |
| GET | /company/candidates/{referenceCode}/documents | Document list — FULL scope only | Yes (Company) |
| POST | /company/documents/{id}/download-url | Issue short-lived watermarked signed URL | Yes (Company) |
| POST | /company/candidates/{referenceCode}/notes | Add private note | Yes (Company) |
| POST | /company/grants/{id}/outcome | Report hiring outcome | Yes (Company) |
| POST | /company/grants/{id}/extension-request | Request window extension (e.g. interview pending) | Yes (Company) |
| GET | /company/dashboard | Active grants, expiring soon, pending outcomes | Yes (Company) |

**Deliberately absent:** there is no `/company/candidates/export`, no `/company/candidates/search` across the full pool, and no public API. Their absence is a requirement, not an omission (FR-610).

---

# Section 8: UI/UX Flow Descriptions

## 8.1 Candidate Registration & Publication Flow

**Actor:** Candidate
**Goal:** Get their profile published and visible to hiring companies

1. Candidate lands on the public landing page and selects "Register as a Job Seeker"
2. Candidate completes the account form (email, password) and submits
3. System sends a verification email; Candidate clicks the link
4. System activates the account and directs the Candidate to the profile wizard
5. Candidate completes Step 1 — Personal Details (name, NIC, contact, city, date of birth)
6. Candidate completes Step 2 — Professional Details (job category, position sought, experience, availability, expected salary)
7. Candidate completes Step 3 — Qualifications and Experience entries
8. Candidate completes Step 4 — Skills and Document Upload
9. System displays Step 5 — a **"How companies will first see you"** preview, showing the masked profile
10. Candidate reviews the masked preview and ticks the explicit consent control (unchecked by default)
11. Candidate submits; system records the consent with terms version, timestamp, and IP
12. System sets status to `Submitted` and confirms with an on-screen message and email
13. Flow ends with the profile in the Admin moderation queue

**Alternative flows / error states:**
- If date of birth indicates under 18, registration is blocked with an explanatory message
- If NIC or email already exists, the system offers a password-reset path rather than confirming an account exists
- If a document fails malware scanning, it is rejected and the Candidate is asked to re-upload
- If the Candidate abandons the wizard, the profile remains in `Draft` and a reminder email is sent after 3 days

The masked-preview step at 9 is deliberate. It sets accurate expectations about what is disclosed, which materially reduces later disputes and complaints.

## 8.2 Admin Moderation Flow

**Actor:** Admin (Operations)
**Goal:** Verify candidate data is genuine and publish it as saleable inventory

1. Admin opens the Moderation Queue, sorted oldest-first
2. Admin selects a submitted profile
3. System displays a split view — full submitted data on the left, the masked company-facing preview on the right
4. Admin verifies qualifications and experience against uploaded documents
5. Admin checks the re-identification risk flag, if raised
6. Admin adjusts employer and institution descriptors where the auto-generated masking is inadequate
7. Admin records a verification note and selects Approve or Reject
8. On Approve, system creates an immutable profile version snapshot and sets status `Approved`
9. Admin publishes; status becomes `Published` and the candidate enters the available pool
10. Flow ends with the Candidate notified by email

**Alternative flows / error states:**
- On Reject, the Admin must select a reason; the Candidate receives it and may edit and resubmit
- If the re-identification flag is raised, the system requires the Admin to acknowledge it explicitly before publishing
- If documents are missing or illegible, the Admin uses a "Request More Information" action, which returns the profile to the Candidate without a formal rejection

## 8.3 Batch Curation & Preview Issue Flow

**Actor:** Admin
**Goal:** Assemble and issue a preview batch to a company after a sales conversation

1. Admin selects the target Company from the company list and chooses "New Batch"
2. Admin applies filters — job category, experience band, city, qualification level
3. System returns matching `Published` candidates, with reserved candidates shown greyed out and unselectable
4. Admin reviews masked previews inline and selects candidates (typically ten)
5. Admin sets the validity window and confirms scope as `PREVIEW`
6. System shows a confirmation summary — candidate count, window, and the reservation warning that these candidates become unavailable to other companies
7. Admin commits the batch
8. System executes the commit in a single transaction, creating one Grant per candidate
9. System sets each candidate to `Reserved` and notifies the Company that a new batch is available
10. Flow ends with the batch in `Issued` status

**Alternative flows / error states:**
- If any selected candidate was reserved concurrently by another Admin, the entire commit fails; the system names the conflicting candidates and returns the Admin to selection with the remainder intact
- If the Company status is not `Active`, commit is blocked with an explanatory message
- If the Company has no recorded data-handling agreement, commit is blocked
- Admin may save the batch as a draft; drafts reserve nothing

## 8.4 Company Preview & Upgrade Request Flow

**Actor:** Company User
**Goal:** Review anonymised candidates and secure contact details for the ones worth interviewing

1. Company user logs in and sees the dashboard with the assigned batch and days remaining
2. User opens the batch and sees ten masked candidate cards, each with a reference code
3. User opens a card and reviews qualifications, experience summary, skills, city, and availability
4. System logs a `preview_view` Access Event
5. User marks candidates of interest and selects "Request Full Access"
6. User optionally adds notes and submits the request
7. System notifies Admin and sets the request status to `Requested`
8. Admin quotes the fee offline; status moves to `Quoted`, then `Awaiting Payment`
9. Company pays offline; Admin records receipt against the Order
10. Admin upgrades the relevant grants; system creates new `FULL` scope grants superseding the previews
11. Company user is notified and can now see name, contact details, and original documents for those candidates
12. Flow ends with the Company able to contact the candidates directly

**Alternative flows / error states:**
- If the user opens a candidate whose grant has lapsed, the system shows an "Access Expired" screen with no cached content rendered
- If the daily view rate limit is reached, further views are blocked and Admin is alerted
- If Admin declines the request, the Company sees the status with an optional reason
- If the payment is never received, the grants remain at `PREVIEW` and lapse normally

## 8.5 Full Access & Document Download Flow

**Actor:** Company User
**Goal:** Retrieve the candidate's CV and contact details

1. User opens an unlocked candidate profile
2. System resolves the grant, confirms `FULL` scope and validity, and returns the full projection
3. System logs a `full_view` Access Event recording the profile version displayed
4. User selects "Download CV"
5. System re-resolves the grant, generates a watermarked copy stamped with company name, user email, and timestamp
6. System issues a signed URL valid for 5 minutes and logs a `document_download` event
7. Browser downloads the watermarked document
8. Flow ends with the file on the user's machine, attributable if it later circulates

**Alternative flows / error states:**
- If the grant lapsed between page load and download, the download is refused
- If the signed URL expires before use, the user must re-request it
- At `PREVIEW` scope, only the redacted CV variant is offered, if one exists

## 8.6 Grant Expiry & Outcome Reporting Flow

**Actor:** System, then Company User, then Admin
**Goal:** Close the loop on every grant and return unhired candidates to the pool

1. Scheduled job identifies grants expiring within 3 days
2. System notifies the Company user, prompting for an outcome
3. Company user reports an outcome from the dashboard
4. If `Hired`, system flags the candidate for Admin placement confirmation
5. If `Interview Pending`, system offers an extension request rather than forcing a final answer
6. On expiry, system transitions the grant to `Lapsed` and returns the candidate to `Published`
7. If no outcome was reported, system generates an Admin follow-up task with a due date
8. Admin contacts the Company by phone and records the outcome manually
9. Admin confirms a `Hired` outcome; system transitions the candidate to `Placed` and notifies them
10. Flow ends with every grant closed and every candidate in a correct state

**Alternative flows / error states:**
- If a follow-up task remains open 7 days past its due date, it escalates to Super Admin
- If the Candidate disputes a `Placed` status, an Admin can revert them to `Published`
- If the Company requests an extension, Admin may extend the window; the extension count is recorded

## 8.7 Candidate Transparency & Withdrawal Flow

**Actor:** Candidate
**Goal:** See who has their data and leave if they choose

1. Candidate opens "Who has seen my profile"
2. System displays preview counts and the named companies holding full access, with dates
3. Candidate may select "Withdraw from platform"
4. System explains clearly what happens — no new companies will be given access; any company that already paid retains access until their window expires
5. Candidate confirms
6. System blocks all new grant issuance, records the consent withdrawal, and notifies any company holding an active grant
7. Flow ends with the candidate withdrawn and the audit trail intact

**Alternative flows / error states:**
- If the Candidate requests full deletion, an Admin task is created and resolved within 30 days
- Where a settled Order references the candidate, the record is anonymised rather than hard-deleted, retaining only the reference code and transactional facts

---

# Section 9: Assumptions & Constraints

## Assumptions

- All payment collection, invoicing, and receipting happens outside the system. The platform is the authoritative record of what was sold and what access was granted, but it never touches money.
- Companies are onboarded only after a physical meeting, and eSupport performs its own commercial due diligence at that point. The system records that verification but does not automate it.
- A signed data-handling agreement is in place with every company before any grant is issued.
- Candidate-supplied qualifications and experience are verified by Admin against uploaded documents during moderation. The system supports that check but does not independently verify credentials with issuing institutions.
- Candidates have reliable email access; email is the primary notification channel at launch.
- The initial candidate pool is small enough that exclusive reservation does not starve supply. This assumption should be monitored — see Constraints.
- English is sufficient for all three user groups at launch.
- eSupport provides all branding assets and the legal text for terms of service and privacy notice.

## Constraints

- **Exclusive reservation throttles throughput.** Because a candidate may hold only one active grant, ten candidates previewed by Company A are invisible to Company B for the full window. With a thin pool this directly limits how many companies can be served concurrently. Two mitigations should be planned for: keeping default preview windows short (7–14 days), and monitoring the available-versus-reserved ratio per category as an operational health metric. If the ratio deteriorates, the exclusivity rule is the first thing to revisit commercially.
- **Masking cannot make a profile truly anonymous.** In a market of Sri Lanka's size, a specific job title plus an experience band plus a city can identify a person. The masking engine and the re-identification risk flag reduce this exposure; they do not eliminate it. This limitation should be stated plainly in the candidate-facing privacy notice.
- **Watermarking deters but does not prevent leakage.** A company that has paid for full access can copy the data. Watermarking makes leaks attributable, which is most of the available deterrent. The commercial contract carries the rest.
- PDPA compliance obligations apply to this processing and are treated as build requirements, not post-launch tasks. Legal review of the terms of service, privacy notice, and company data-handling agreement should be commissioned in parallel with development.
- No public API is exposed. Any future integration requirement will need a separate, separately-secured design.
- Mobile applications, CV parsing, and multilingual UI are out of scope for the initial release; the architecture accommodates them without rework.
- The system depends on Admin discipline for outcome tracking. The follow-up queue makes the work visible and escalates neglect, but it cannot force a company to answer the phone.

---

# Section 10: Revision History

| Version | Date | Author | Changes |
|---|---|---|---|
| v1.0 | 28 July 2026 | Sadeeka — eSupport Technology | Initial document. Confirms exclusive reservation model, flat unit pricing, time-bounded grants, admin-only company onboarding, and offline payment recording |

---

*eSupport Technology (Pvt) Ltd | prasad@esupport.live | esupport.live*
*Confidential*
